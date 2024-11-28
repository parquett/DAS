using Controls.Link;
namespace Controls.Link
{
    using System;
    using System.Net.Cache;

    using Controls.Link.Models;
    using Lib.AdvancedProperties;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Globalization;
    using Lib.Tools.Utils;
    using Lib.Tools.BO;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using lib;

    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {
            var model = new Controls.Link.Models.LinkModel();

            //General.TraceWrite(property.PropertyName);      
            try
            {
                model.ReadOnly = ReadOnly;
                var url = "";
                var Caption = property.Common.DisplayName;
                var Module = "ControlPanel";
                if(HttpContextHelper.Current!=null){
                    var context = HttpContextHelper.Current;
                    if (!string.IsNullOrEmpty(context.Session.GetString(SessionItems.Module)))
                        Module = context.Session.GetString(SessionItems.Module);
                }
                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.Link:
                        url = URLHelper.GetUrl(Module + "/Edit/" + property.PropertyName.Remove(property.PropertyName.Length - 1) + "/" + System.Net.WebUtility.UrlEncode(BOItem.GetType().Namespace) + "/?" + BOItem.GetType().Name + "Id=" + BOItem.GetId().ToString());
                        if (value as ItemBase != null)
                        {
                            var item = (ItemBase)value;
                            Caption = item.GetLinkName();
                            url = URLHelper.GetUrl(item.GetLink());
                        }
                        model.Link = new Lib.Models.LinkModel() { Caption = Caption, Href = url };
                        break;
                    case EditTemplates.LinkItem:
                        if (value != null)
                        {
                            var item = (ItemBase)value;
                            url = URLHelper.GetUrl(Module.Substring(1, Module.Length - 2) + "/EditItem/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace) + "/" + item.Id.ToString());
                            model.Link = new Lib.Models.LinkModel() { Caption = item.GetReportGridName(), Href = url };
                            model.Name = property.Common.DisplayName;
                            model.Id = item.Id.ToString();
                        }
                        break;
                    case EditTemplates.Parent:
                        if (value != null)
                        {
                            var item = (ItemBase)value;
                            //item = item.PopulateOne(item);
                            Caption = item.GetLinkName();
                            url = URLHelper.GetUrl(Module.Substring(1, Module.Length - 2) + "/EditItem/" + item.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(item.GetType().Namespace) + "/" + item.Id.ToString());
                            model.Link = new Lib.Models.LinkModel() { Caption = Caption, Href = url };
                            model.Name = property.PropertyName;
                            model.Id = item.Id.ToString();
                        }
                        break;
                    case EditTemplates.LinkItems:
                        if ((property.Custom as LinkItem).LinkType != null)
                        {
                            url = URLHelper.GetUrl(Module.Substring(1, Module.Length - 2) + "/Edit/" + (property.Custom as LinkItem).LinkType.Name + "/" + System.Net.WebUtility.UrlEncode((property.Custom as LinkItem).LinkType.Namespace) + "/" + BOItem.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(BOItem.GetType().Namespace) + "/" + BOItem.Id.ToString());
                        }
                        else
                        {
                            url = URLHelper.GetUrl(Module.Substring(1, Module.Length - 2) + "/Edit/" + (property.Custom as LinkItem).Class + "/" + System.Net.WebUtility.UrlEncode((property.Custom as LinkItem).Namespace) + "/" + BOItem.GetType().Name + "/" + System.Net.WebUtility.UrlEncode(BOItem.GetType().Namespace) + "/" + BOItem.Id.ToString());
                        }
                        model.Link = new Lib.Models.LinkModel() { Caption = Caption, Href = url };
                        break;
                    default:
                        break;
                }

                model.Mode = mode;
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
            }

            return model;
        }

        public object GetValue(AdvancedProperty property, string prefix = "", DisplayMode mode = DisplayMode.Simple)
        {
            try
            {
                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.Link:
                        // tbd
                        break;
                    case EditTemplates.LinkItem:
                    case EditTemplates.Parent:                    
                        var item = (ItemBase)Activator.CreateInstance(property.Type);
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        {
                            item.SetId(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]);
                        }
                        return item;
                    case EditTemplates.LinkItems:
                        return new Dictionary<long, ItemBase>();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
            }

            return null;
        }
        
        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";

            switch (property.Common.EditTemplate)
            {
                case EditTemplates.Link:
                    if (value as ItemBase != null)
                    {
                        var item = (ItemBase)value;
                        strValue = item.GetLinkName();
                    }
                    break;
                case EditTemplates.LinkItem:
                case EditTemplates.Parent:
                    if (value != null)
                    {
                        var item = (ItemBase)value;
                        strValue = item.GetLinkName();
                    }
                    break;
                case EditTemplates.LinkItems:
                    strValue="";
                    break;
                default:
                    break;
            }

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
