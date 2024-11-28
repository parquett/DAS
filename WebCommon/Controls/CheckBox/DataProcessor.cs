using Controls.CheckBox;
namespace Controls.CheckBox
{
    using System;
    using System.Net.Cache;

    using Controls.CheckBox.Models;
    using Lib.AdvancedProperties;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Globalization;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using lib;

    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {
            var model = new CheckBoxModel();

            //General.TraceWrite(property.PropertyName);
            try
            {
                model.ReadOnly = ReadOnly;

                if (value == null)
                    value = false;
                model.Value = (bool)value;

                model.Checkbox = new CheckboxModel() { Name = property.PropertyName,Id = new Random().Next().ToString(), Checked = model.Value, Class = "flat-red" };
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
                return HttpContextHelper.Current.Request.Form[property.PropertyName] == "1";
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
            }

            return false;
        }

        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";
            if (value == null)
                value = false;

            if ((bool)value)
                strValue = "Yes";
            else
                strValue = "No";

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            return ToString(value, property, BOItem, mode);
        }
    }
}
