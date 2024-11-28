using Controls.File;
namespace Controls.File
{
    using System;
    using System.Net.Cache;

    using Controls.File.Models;
    using Lib.AdvancedProperties;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Globalization;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using Lib.BusinessObjects;
    using lib;

    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {
            var model = new FileModel();

            //General.TraceWrite(property.PropertyName);
            try
            {
                model.ReadOnly = ReadOnly;

                if (value != null && ((Document)value).Id > 0)
                    model.Value = (Document)value;
                else
                {
                    model.Value = new Document();
                }

                model.CssView = property.Common.ViewCssClass;
                model.CssEdit = property.Common.EditCssClass;
                model.BOName = BOItem.GetType().Name;
                model.UniqueId = property.PropertyName + "_" + BOItem.Id.ToString();
                model.PropertyName = property.PropertyName;
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

            //General.TraceWrite(property.PropertyName);

            try
            {
                return new Document(Convert.ToInt64(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]));
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
            }

            return new Document();
        }
        
        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return "";
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
