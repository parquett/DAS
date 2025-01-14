﻿using Controls.NumberRange;
namespace Controls.NumberRange
{
    using System;
    using System.Net.Cache;

    using Controls.NumberRange.Models;
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
            var model = new NumberRangeModel();

            //General.TraceWrite(property.PropertyName);
            try
            {
                model.ReadOnly = ReadOnly;

                var TextboxTypeVal = TextboxType.Number;

                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.NumberRange:
                        model.Value = value != null && ((NumbersRange)value).from != ((NumbersRange)value).to ? ((NumbersRange)value).from.ToString() : "";

                        model.TextBoxFrom = new TextboxModel() { Name = property.PropertyName + "From", Class = "input input-numberrange", Type = TextboxTypeVal, Value = model.Value };
                        model.TextBoxTo = new TextboxModel() { Name = property.PropertyName + "To", Class = "input input-numberrange", Type = TextboxTypeVal, Value = (value != null ? ((NumbersRange)value).to.ToString() : "") };
                        break;
                    case EditTemplates.DecimalNumberRange:
                        model.Value = value != null && ((DecimalNumberRange)value).from != ((DecimalNumberRange)value).to ? ((DecimalNumberRange)value).from.ToString() : "";

                        model.TextBoxFrom = new TextboxModel() { Name = property.PropertyName + "From", Class = "input input-numberrange", Type = TextboxTypeVal, Value = model.Value };
                        model.TextBoxTo = new TextboxModel() { Name = property.PropertyName + "To", Class = "input input-numberrange", Type = TextboxTypeVal, Value = (value != null ? ((DecimalNumberRange)value).to.ToString() : "") };
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
            //General.TraceWrite(property.PropertyName);

            switch (property.Common.EditTemplate)
            {
                case EditTemplates.NumberRange:
                    Lib.Tools.Controls.NumbersRange range = new Lib.Tools.Controls.NumbersRange();

                    try
                    {
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "From"]))
                        {
                            range.from = Convert.ToInt32(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "From"]);
                        }
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "To"]))
                        {
                            range.to = Convert.ToInt32(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "To"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        General.TraceWarn(ex.ToString());

                        if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                            ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
                    }

                    return range;
                case EditTemplates.DecimalNumberRange:
                    Lib.Tools.Controls.DecimalNumberRange decimalrange = new Lib.Tools.Controls.DecimalNumberRange();

                    try
                    {
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "From"]))
                        {
                            decimalrange.from = Convert.ToDecimal(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "From"]);
                        }
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "To"]))
                        {
                            decimalrange.to = Convert.ToDecimal(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "To"]);
                        }
                    }
                    catch (Exception ex)
                    {
                        General.TraceWarn(ex.ToString());

                        if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                            ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
                    }

                    return decimalrange;
            }

            return null;
        }
        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";

            if (value == null)
                return "";

            dynamic NumbersRange = value;

            if (NumbersRange.from != 0 && NumbersRange.to != 0)
                strValue = NumbersRange.from.ToString() + "-" + NumbersRange.to.ToString();
            else if (NumbersRange.from != 0)
                strValue = NumbersRange.from.ToString();
            else if (NumbersRange.to != 0)
                strValue = NumbersRange.to.ToString();

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            if (value != null)
            {
                dynamic NumbersRange = value;
                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.NumberRange:
                        return NumbersRange.from;
                    case EditTemplates.DecimalNumberRange:
                        return NumbersRange.from;
                }
            }
            return 0;
        }
    }
}
