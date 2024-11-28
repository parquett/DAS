// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemBase.cs" company="SecurityCRM">
//   Copyright ©  2018
// </copyright>
// <summary>
//   Defines the ItemBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Lib.AdvancedProperties;
using Lib.BusinessObjects.Translations;
using Lib.Tools.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Lib.Tools.BO
{
    public class ItemBasePropertyConverter : JsonConverter
    {
        public ItemBasePropertyConverter()
        {
        }

        private readonly Type _type;

        public ItemBasePropertyConverter(Type type)
        {
            _type = type;
        }

        public override bool CanConvert(Type objectType)
        {
            // this converter can be applied to any type
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            var item = (ItemBase)Activator.CreateInstance(_type!=null? _type: objectType);

            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jsonItem = JObject.Load(reader);

                if (jsonItem["Id"] != null) {

                    item.SetId(jsonItem["Id"].Value<long>());

                    if (objectType == typeof(Language))
                    {
                        ((Language)item).ShortName=jsonItem["ShortName"].Value<string>();
                        return item;
                    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var properties = pss.GetDbProperties(pdc,null);

                    foreach (AdvancedProperty property in properties)
                    {
                        if (jsonItem[property.PropertyName] != null)
                        {
                            if (property.Type == typeof(ItemBase)
                                || (property.Type.BaseType == typeof(ItemBase))
                                || (property.Type.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                )
                            { 

                                var Child = JsonConvert.DeserializeObject(jsonItem[property.PropertyName].ToString(), property.Type);

                                property.PropertyDescriptor.SetValue(item, Child);
                            }
                            else
                            {
                                property.PropertyDescriptor.SetValue(item, jsonItem[property.PropertyName].ToObject(property.Type));
                            }
                        }
                    }
                }
            }
            return item;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                serializer.Serialize(writer, null);
                return;
            }

            // find all properties with type 'int'
            //var properties = value.GetType().GetProperties();//.Where(p => p.PropertyType == typeof(ItemBase));
            if (value as ItemBase != null)
            {
                writer.WriteStartObject();

                writer.WritePropertyName("Id");
                writer.WriteValue((value as ItemBase).Id);

                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(value.GetType());
                var properties = pss.GetDbProperties(pdc,null);

                foreach (AdvancedProperty property in properties)
                {
                    if (string.IsNullOrEmpty(property.PropertyName))
                    {
                        continue;
                    }
                    var lValue = property.PropertyDescriptor.GetValue(value);
                    if (lValue == null)
                    {
                        continue;
                    }
                    else if (lValue is Dictionary<long, ItemBase>)
                    {
                        continue;
                    }
                    else if (lValue as ItemBase == null)
                    {
                        if (lValue == null)
                            continue;

                        if (lValue is DateTime && (DateTime)lValue == DateTime.MinValue)
                            continue;

                        if (string.IsNullOrEmpty(lValue.ToString()))
                            continue;
                    }
                    else if (((ItemBase)lValue).Id == 0)
                    {
                        continue;
                    }

                    // write property name
                    writer.WritePropertyName(property.PropertyName);
                    // let the serializer serialize the value itself
                    // (so this converter will work with any other type, not just int)
                    serializer.Serialize(writer, lValue);
                }

                writer.WriteEndObject();
            }
            else
            {
                try
                {
                    writer.WriteValue(value);
                }
                catch(Exception ex)
                {
                    //TBD
                }
            }
        }
    }
}