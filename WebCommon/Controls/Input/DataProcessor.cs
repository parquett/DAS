using Controls.Input;
namespace Controls.Input
{
    using System;
    using System.Net.Cache;

    using Controls.Input.Models;
    using Lib.AdvancedProperties;
    using Lib.Tools.Controls;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Globalization;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using Lib.BusinessObjects;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using lib;
    using SecurityCRMLib.Security;

    public class DataProcessor : IDataProcessor
    {
        public object SetValue(object value, AdvancedProperty property, ItemBase BOItem, bool ReadOnly = false, DisplayMode mode = DisplayMode.Simple)
        {
            var model = new InputModel() { Mode = mode };

            try
            {
                model.ReadOnly = ReadOnly
                                ||
                                (property.Access.EditableFor != (long)SecurityCRM.ApiContracts.Enums.Permissions.None && !(Lib.Tools.Security.Authentication.GetCurrentUser().HasAtLeastOnePermission(property.Access.EditableFor)))
                                ||
                                (property.Db != null && property.Db.Editable == false && mode != DisplayMode.FrontEnd)
                                ;

                var TextboxTypeVal = TextboxType.Text;
                var InputVal = "";
                var useDefault = property.Custom != null && property.Custom is LookUp && ((LookUp)property.Custom).DefaultValue;

                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.DateInput:
                        TextboxTypeVal = TextboxType.Date;
                        if (useDefault && mode == DisplayMode.Search)
                        {
                            model.Value = "";
                        }
                        else
                        {
                            DateTime origdate = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTime.MinValue;
                            DateTime date = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : (useDefault ? DateTime.MinValue : DateTimeZone.Now);
                            model.Value = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            InputVal = (date != DateTime.MinValue) ? date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";

                            if (origdate == DateTime.MinValue) model.Value = "";
                        }
                        break;

                    case EditTemplates.DateTimeInput:

                        TextboxTypeVal = TextboxType.DateTime;
                        model.HourTextBox = new TextboxModel();
                        model.MinutesTextBox = new TextboxModel();

                        if (useDefault && mode == DisplayMode.Search)
                        {
                            model.Value = "";
                        }
                        else
                        {
                            DateTime origdatetime = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTime.MinValue;
                            DateTime datetime = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : (useDefault ? DateTime.MinValue : DateTimeZone.Now);
                            model.Value = datetime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                            InputVal = (datetime != DateTime.MinValue) ? datetime.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                            if (origdatetime == DateTime.MinValue) model.Value = "";

                            model.HourTextBox = new TextboxModel() { Name = property.PropertyName + "_Hours", Type = TextboxTypeVal, Value = datetime.ToString("HH", CultureInfo.InvariantCulture), Class = "input input-small", MaxLength = 2, OnKeyUp = "isHourKey(this)", OnKeyPress = "return isNumberKey(event)" };
                            model.MinutesTextBox = new TextboxModel()
                            {
                                Name = property.PropertyName + "_Minutes",
                                Type = TextboxTypeVal,
                                Value = datetime.ToString("mm", CultureInfo.InvariantCulture),
                                Class = "input input-small",
                                MaxLength = 2,
                                OnKeyUp = "isMinuteKey(this)",
                                OnKeyPress = "return isNumberKey(event)"
                            };
                        }
                        break;

                    case EditTemplates.Password:
                        model.Value = "";
                        TextboxTypeVal = TextboxType.Password;
                        break;

                    case EditTemplates.MultiLine:
                        TextboxTypeVal = TextboxType.MultiLine;
                        model.Value = value != null ? value.ToString() : "";
                        break;

                    case EditTemplates.HtmlInput:
                        TextboxTypeVal = TextboxType.HTML;
                        model.Value = value != null ? value.ToString() : "";
                        break;

                    case EditTemplates.Label:
                    case EditTemplates.NotUpdatableInput:
                        model.ReadOnly = true;
                        model.Value = value != null ? value.ToString() : "";
                        break;

                    default:
                        model.Value = value != null ? value.ToString() : "";
                        break;
                }

                model.TextBox = new TextboxModel() { Name = property.PropertyName, Type = TextboxTypeVal, Value = !string.IsNullOrEmpty(InputVal) ? InputVal : model.Value };
                if (property.Custom is InputBox)
                {
                    //General.TraceWarn("OnChange: " + ((InputBox)property.Custom).OnChange);
                    if (!string.IsNullOrEmpty(((InputBox)property.Custom).OnChange))
                    {
                        model.TextBox.OnChange = ((InputBox)property.Custom).OnChange;
                    }
                }
                //General.TraceWarn("model.TextBox.OnChange: " + model.TextBox.OnChange);
                if (property.Validation != null)
                {
                    if (mode != DisplayMode.Search)
                    {
                        model.TextBox.ValidationType = property.Validation.ValidationType;
                    }
                    model.TextBox.RegularExpression = property.Validation.RegularExpression;
                    model.TextBox.ValidationFuction = property.Validation.ValidationFunction;
                    if (!string.IsNullOrEmpty(property.Validation.AlertMessage))
                        model.TextBox.RequiredMessage = property.Validation.AlertMessage;
                }
                model.Label = property.Common.DisplayName;

                if (property.Common.EditTemplate == EditTemplates.DateInput
                    || property.Common.EditTemplate == EditTemplates.DateTimeInput)
                {
                    model.TextBox.Class = "input calendar-input small-input";
                    model.CssEdit = " control-input-date";
                }
                else if (property.Common.EditTemplate == EditTemplates.DiagnosticInput)
                {
                    model.TextBox.Class = "input diagn-input";
                }
                else if (property.Common.EditTemplate == EditTemplates.ColorPicker)
                {
                    model.TextBox.Class = "input colorpicker-input";
                    model.CssView = "colorpicker-view ";
                    model.Color = model.Value;
                }
                else
                {
                    model.TextBox.Class = property.Common.ControlClass;
                }

                model.CssView += property.Common.ViewCssClass;
                model.CssEdit += property.Common.EditCssClass;
                model.Mode = mode;


                if (property.Translate != null && property.Translate.Translatable)
                {
                    model.Translatable = true;
                    model.BOName = BOItem.GetType().Name;
                    model.BOId = BOItem.Id;
                    model.PropertyName = property.PropertyName;
                }

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
                switch (property.Common.EditTemplate)
                {
                    case EditTemplates.DateInput:

                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        {
                            return DateTime.ParseExact(
                                HttpContextHelper.Current.Request.Form[prefix + property.PropertyName][0],
                                @"dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        return DateTime.MinValue;

                    case EditTemplates.HtmlPopUpInput:
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_fullpopupvalue"]))
                        {
                            return HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_fullpopupvalue"].ToString();
                        }
                        return "";

                    case EditTemplates.HtmlInput:
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_fullpopupvalue"]))
                        {
                            return HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_fullpopupvalue"].ToString();
                        }
                        return HttpContextHelper.Current.Request.Form[prefix + property.PropertyName].ToString();

                    case EditTemplates.DateTimeInput:
                        if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        {
                            var date = !string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]) ? DateTime.ParseExact(
                                       HttpContextHelper.Current.Request.Form[prefix + property.PropertyName].ToString(),
                                        @"dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.MinValue;
                            if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_Hours"]))
                            {
                                date = date.AddHours(Convert.ToInt32(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_Hours"]));
                            }
                            if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_Minutes"].ToString()))
                            {
                                date = date.AddMinutes(Convert.ToInt32(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName + "_Minutes"].ToString()));
                            }
                            return date;
                        }
                        return DateTime.MinValue;

                    case EditTemplates.Password:
                        return GetpasswordHash(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName][0], "");

                }
                if (property.Type == typeof(decimal))
                {
                    if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        return Convert.ToDecimal(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName].ToString().Replace(",", "."), CultureInfo.InvariantCulture);

                    return (decimal)0;
                }
                if (property.Type == typeof(int) || property.Type == typeof(int?))
                {
                    if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        return Convert.ToInt32(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName].ToString());

                    return 0;
                }
                if (property.Type == typeof(long))
                {
                    if (!string.IsNullOrEmpty(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName]))
                        return Convert.ToInt64(HttpContextHelper.Current.Request.Form[prefix + property.PropertyName].ToString());

                    return 0;
                }
                return HttpContextHelper.Current.Request.Form[prefix + property.PropertyName][0].ToString();
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());

                if (Config.GetConfigValue("SendExceptionsByAPI") == "1")
                    ExceptionManagement.HandleExceptionByAPI(ex, HttpContextHelper.Current);
            }

            return "";
        }


        /// <summary>
        /// The getpassword hash.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetpasswordHash(string Password, string salt)
        {
            try
            {
                string _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
                int _iteration = 3;
                var passwordHash = PasswordHasher.ComputeHash(Password, salt, _pepper, _iteration);
                
                return passwordHash;
            }
            catch (Exception ex)
            {
                General.TraceWarn(ex.ToString());
                return ex.ToString();
            }
        }

        public string ToString(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            var strValue = "";

            switch (property.Common.EditTemplate)
            {
                case EditTemplates.DateInput:

                    DateTime date = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTimeZone.Now;
                    strValue = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (date == DateTime.MinValue) strValue = "";
                    break;

                case EditTemplates.DateTimeInput:

                    DateTime datetime = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTimeZone.Now;
                    strValue = datetime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    if (datetime == DateTime.MinValue) strValue = "";
                    break;

                case EditTemplates.Password:
                    strValue = "";
                    break;

                case EditTemplates.MultiLine:
                case EditTemplates.HtmlInput:
                case EditTemplates.Label:
                case EditTemplates.NotUpdatableInput:
                    strValue = value != null ? value.ToString() : "";
                    if (mode == DisplayMode.CSV)
                        strValue = strValue.Replace("&lt;br/&gt;", ";");
                    else
                        strValue = strValue.Replace("&lt;br/&gt;", "\n").TrimEnd(new char[] { '\r', '\n' });

                    strValue = strValue.Replace("&lt;", "<");
                    strValue = strValue.Replace("&gt;", ">");
                    break;

                default:
                    if (property.Type == typeof(decimal))
                    {
                        strValue = value != null ? ((decimal)value).ToString(CultureInfo.InvariantCulture) : "0";
                    }
                    else if (property.Type == typeof(int) || property.Type == typeof(int?))
                    {
                        strValue = value != null ? ((int)value).ToString(CultureInfo.InvariantCulture) : "0";
                    }
                    else if (property.Type == typeof(long))
                    {
                        strValue = value != null ? ((long)value).ToString(CultureInfo.InvariantCulture) : "0";
                    }
                    else
                    {
                        strValue = value != null ? value.ToString() : "";
                    }
                    if (mode == DisplayMode.CSV)
                        strValue = strValue.Replace("&lt;br/&gt;", ";");
                    else
                        strValue = strValue.Replace("&lt;br/&gt;", "\n").TrimEnd(new char[] { '\r', '\n' });
                    strValue = strValue.Replace("&lt;", "<");
                    strValue = strValue.Replace("&gt;", ">");
                    break;
            }

            return strValue;
        }

        public object ToObject(object value, AdvancedProperty property, ItemBase BOItem, DisplayMode mode = DisplayMode.Print)
        {
            object strValue;

            switch (property.Common.EditTemplate)
            {
                case EditTemplates.DateInput:

                    DateTime date = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTimeZone.Now;
                    strValue = date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (date == DateTime.MinValue) strValue = "";
                    break;

                case EditTemplates.DateTimeInput:

                    DateTime datetime = value is DateTime && (DateTime)value != DateTime.MinValue ? (DateTime)value : DateTimeZone.Now;
                    strValue = datetime.ToString("dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    if (datetime == DateTime.MinValue) strValue = "";
                    break;

                case EditTemplates.Password:
                    strValue = "";
                    break;

                case EditTemplates.MultiLine:
                case EditTemplates.HtmlInput:
                case EditTemplates.Label:
                case EditTemplates.NotUpdatableInput:
                    strValue = value != null ? value.ToString() : "";
                    if (mode == DisplayMode.CSV)
                        strValue = strValue.ToString().Replace("&lt;br/&gt;", ";");
                    else
                        strValue = strValue.ToString().Replace("&lt;br/&gt;", "\n").TrimEnd(new char[] { '\r', '\n' });

                    strValue = strValue.ToString().Replace("&lt;", "<");
                    strValue = strValue.ToString().Replace("&gt;", ">");
                    break;

                default:
                    if (property.Type == typeof(decimal))
                    {
                        return (decimal)value;
                    }
                    else if (property.Type == typeof(int) || property.Type == typeof(int?))
                    {
                        return (int)value;
                    }
                    else if (property.Type == typeof(long))
                    {
                        return (long)value;
                    }
                    else
                    {
                        strValue = value != null ? value.ToString() : "";
                    }
                    if (mode == DisplayMode.CSV)
                        strValue = strValue.ToString().Replace("&lt;br/&gt;", ";");
                    else
                        strValue = strValue.ToString().Replace("&lt;br/&gt;", "\n").TrimEnd(new char[] { '\r', '\n' });
                    strValue = strValue.ToString().Replace("&lt;", "<");
                    strValue = strValue.ToString().Replace("&gt;", ">");
                    break;
            }

            return strValue.ToString().Replace("&nbsp;", "").Trim();
        }
    }
}
