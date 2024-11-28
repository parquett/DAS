using Lib.Tools.BO;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Weblib.Helpers;
using Lib.BusinessObjects;
using SecurityCRMLib.BusinessObjects;
using System.Globalization;

using System.IO;
using Lib.AdvancedProperties;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using SecurityCRM.Models.Reports;
using Weblib.Models.Common;
using Lib.Helpers;
using SecurityCRMLib.Security;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Lib.Plugins;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Vml;
using DocumentFormat.OpenXml.EMMA;
using lib;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace SecurityCRM.Controllers
{
    [Route("Report")]
    public class ReportController : SecurityCRMweblib.Controllers.ReportObjectController
    {
        public ReportController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        [HttpGet]
        [Route("{Model}/{BOLink?}/{NamespaceLink?}/{Id?}")]
        public ActionResult View(string Model, string BOLink = "ItemBase", string NamespaceLink = "Lib.BusinessObjects", string Id = "")
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var sSearch = "";

            var is_search = false;
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("SecurityCRM.Models.Reports." + Model, true));

            var usr = Authentication.GetCurrentUser();

            if (!Authorization.hasPageAccess(usr, item))
            {
                return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
            }
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var search_properties = pss.GetFilterControlProperties(pdc, Authentication.GetCurrentUser());
            var lookup_properties = pss.GetSearchProperties(pdc);
            Dictionary<long,ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
            AdvancedProperties properties = null; ;

            if (HttpContextHelper.Current.Session.TryGetValue("Display_" + Model, out var sval))
            {
                properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), JsonConvert.DeserializeObject<List<string>>(System.Text.Encoding.Default.GetString(sval)));
            }
            else if (DBProperties != null && DBProperties.Count > 0)
            {
                //properties = new AdvancedProperties();
                //var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                //foreach (AdvancedProperty property in tprops)
                //{
                //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                //    {
                //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                //        property.Common.DisplayName = DBField.Name;
                //        property.Common.PrintName = DBField.PrintName;

                //        if (DBField.DisplayModes != null && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Simple) && usr.HasAtLeastOnePermission(DBField.Permission))
                //        {
                //            properties.Add(property);
                //        }
                //    }
                //}
                //properties.Sort();
            }
            else
            {
                properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
            }

            ViewData["Model"] = Model;

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
            {
                boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                {
                    ViewBag.Title = "SecurityCRM: " + boproperties.DisplayName;
                    ViewData["Report_Name"] = boproperties.DisplayName;
                    ViewData["NoLink"] = boproperties.NoLink;
                    ViewData["ActionOnClick"] = boproperties.ActionOnClick;
                }
                if (boproperties != null)
                {
                    ViewData["NewTab"] = boproperties.OpenInNewTab;
                }
                else
                {
                    ViewData["NewTab"] = false;
                }
            }
            else
            {
                ViewData["NewTab"] = false;
                ViewData["NoLink"] = false;
                ViewData["ActionOnClick"] = false;
            }

            General.TraceWarn("ID:" + Id);
            if (!string.IsNullOrEmpty(Id))
            {
                if (!string.IsNullOrEmpty(NamespaceLink) && NamespaceLink != "null")
                {
                    var LinkItem = Activator.CreateInstance(Type.GetType(NamespaceLink + "." + BOLink + ", " + NamespaceLink.Split('.')[0], true));
                    ((ItemBase)LinkItem).Id = Convert.ToInt64(Id);
                    foreach (AdvancedProperty property in lookup_properties)
                    {
                        if (
                            (property.Common.EditTemplate == EditTemplates.Parent
                            || property.Common.EditTemplate == EditTemplates.DropDownParent
                            || property.Common.EditTemplate == EditTemplates.SelectListParent)
                            && property.Type.Name == LinkItem.GetType().Name
                            )
                        {
                            General.TraceWarn("LinkItem.GetType().Name:" + LinkItem.GetType().Name);
                            is_search = true;
                            property.PropertyDescriptor.SetValue(item, LinkItem);
                            break;
                        }
                    }
                }
                else
                {
                    foreach (AdvancedProperty property in lookup_properties)
                    {
                        General.TraceWarn("roperty.PropertyName:" + property.PropertyName);
                        if (property.PropertyName == BOLink)
                        {
                            General.TraceWarn("Bingo!!!");
                            is_search = true;
                            property.PropertyDescriptor.SetValue(item, Convert.ChangeType(Id, property.Type));
                            break;
                        }
                    }
                }
            }

            is_search = item.DefaultReportFilter(is_search);

            if (!string.IsNullOrEmpty(Request.Query["s"]))
            {
                sSearch = Request.Query["s"];
                is_search = true;
            }

            ViewData["sSearch"] = sSearch;
            sSearch = item.SimpleSearch(sSearch);

            ViewData["Breadcrumbs"] = item.LoadBreadcrumbs();
            ViewData["QuickLinks"] = item.LoadQuickLinks();
            ViewData["ReportMenu"] = item.LoadContextReports();

            long itotal;
            long idisplaytotal;

            var DateStart = DateTimeZone.Now;
            var Items = item.PopulateReport(null, is_search ? item : null, 0, boproperties.RecordsPerPage, sSearch, null, null, Lib.AdvancedProperties.DisplayMode.Simple, out itotal, out idisplaytotal);
            ViewData["ReportWidget"] = item.PopulateReportWidget(is_search ? item : null, sSearch, ControllerContext, ViewData, TempData);
            var TimeSpent = DateTimeZone.Now - DateStart;
            ViewData["ReuquestExecTime"] = TimeSpent.TotalMinutes.ToString("0") + " " + Helpers.T.Str("ShortMinutes", "FrontEnd", "min") + " " + TimeSpent.Seconds + " " + Helpers.T.Str("ShortSecond", "FrontEnd", "sec");

            var redirectUrl = "";
            if (Items.Count == 1 && item.ReportSingleItemRedirect(Items.Values.FirstOrDefault(i => i.Id > 0), out redirectUrl))
            {
                return Redirect(redirectUrl);
            }

            ViewData["Count"] = idisplaytotal;
            ViewData["CountPerPage"] = boproperties.RecordsPerPage;
            ViewData["PageNum"] = 0;
            ViewData["BuildPaginng"] = BuildPaginng(idisplaytotal, boproperties.RecordsPerPage, 0);

            ViewData["DataItems"] = Items;

            ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
            ViewData["Search_Item"] = ((ReportBase)item).getSearchItem(item);

            ViewData["Search_Properties"] = search_properties;
            ViewData["Properties"] = properties;

            return View();
        }

        [HttpPost]
        [Route("OptionsSave/{Model}")]
        public ActionResult OptionsSave(string Model)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var item = (ItemBase)Activator.CreateInstance(Type.GetType("SecurityCRM.Models.Reports." + Model, true));
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
            var DisplayProperties = new List<string>();
            var PrintProperties = new List<string>();
            foreach (AdvancedProperty property in properties)
            {
                if (Request.Form["Display_" + property.PropertyName] == "1")
                {
                    DisplayProperties.Add(property.PropertyName);
                }
                if (Request.Form["Print_" + property.PropertyName] == "1")
                {
                    PrintProperties.Add(property.PropertyName);
                }
            }
            var str = JsonConvert.SerializeObject(PrintProperties);
            SessionHelper<string>.Push("Print_" + Model, str);
            str = JsonConvert.SerializeObject(DisplayProperties);
            SessionHelper<string>.Push("Display_" + Model, str);

            return this.Json(new RequestResult() { Result = RequestResultType.Success });
        }

        //[HttpPost]
        //[Route("Options/{Model}")]
        public ActionResult Options(string Model)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            var usr = Authentication.GetCurrentUser();
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("SecurityCRM.Models.Reports." + Model, true));
            Dictionary<long, ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
            var pss = new PropertySorter();
            var pdc = TypeDescriptor.GetProperties(item.GetType());
            var properties = pss.GetAvailableProperties(pdc, usr);

            AdvancedProperties DisplayProperties = null;
            AdvancedProperties PrintProperties = null;

            if (DBProperties != null && DBProperties.Count > 0)
            {
                //DisplayProperties = new AdvancedProperties();
                //PrintProperties = new AdvancedProperties();
                //foreach (AdvancedProperty property in properties)
                //{
                //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                //    {
                //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                //        property.Common.DisplayName = DBField.Name;
                //        property.Common.PrintName = DBField.PrintName;

                //        if (DBField.DisplayModes != null
                //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Simple)
                //            && usr.HasAtLeastOnePermission(DBField.Permission)
                //            && !HttpContextHelper.Current.Session.TryGetValue("Display_" + Model, out var strD1))
                //        {
                //            DisplayProperties.Add(property);
                //        }

                //        if (DBField.DisplayModes != null
                //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Print)
                //            && usr.HasAtLeastOnePermission(DBField.Permission)
                //            && !HttpContextHelper.Current.Session.TryGetValue("Print_" + Model, out var strP1))
                //        {
                //            PrintProperties.Add(property);
                //        }
                //    }
                //}
                //if (HttpContextHelper.Current.Session.TryGetValue("Display_" + Model, out var strD))
                //{
                //    DisplayProperties = pss.GetAvailableProperties(pdc, usr, JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                //}
                //if (HttpContextHelper.Current.Session.TryGetValue("Print_" + Model, out var strP))
                //{
                //    PrintProperties = pss.GetAvailableProperties(pdc, usr, JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strP)));
                //}
            }
            else
            {
                if (HttpContextHelper.Current.Session.TryGetValue("Display_" + Model, out var strD))
                {
                    DisplayProperties = pss.GetAvailableProperties(pdc, usr, JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                }
                else
                {
                    DisplayProperties = pss.GetProperties(pdc, usr);
                }
                if (HttpContextHelper.Current.Session.TryGetValue("Print_" + Model, out var strP))
                {
                    PrintProperties = pss.GetAvailableProperties(pdc, usr, JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strP)));
                }
                else
                {
                    PrintProperties = pss.GetPrintProperties(pdc, usr);
                }
            }

            ViewData["Properties"] = properties;
            ViewData["DisplayProperties"] = DisplayProperties;
            ViewData["PrintProperties"] = PrintProperties;

            return View();
        }
        private string BuildPaginng(long Count, int CountPerPage, int PageNum)
        {
            var pagingstr = "";

            if (Count / CountPerPage <= 6)
            {
                for (int p = 1; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else if (PageNum <= 3 || PageNum >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                pagingstr += "<li>";
                pagingstr += "<a href='#' onclick='return show_report_page(3)' class='pagination-page'>...</a>";
                pagingstr += "</li>";

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }
            else
            {
                for (int p = 1; p <= 3; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                var pstart = PageNum - 1;
                var pend = PageNum + 1;

                if (pstart <= 3)
                {
                    pstart++;
                    pend++;
                }
                else if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pstart--;
                    pend--;
                }

                if (pstart <= 3)
                {
                    pstart++;
                }

                if (pend >= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2)
                {
                    pend--;
                }

                if (pstart != 4)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(3)' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = pstart; p <= pend; p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }

                if (pend != Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 3)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2).ToString() + ")' ' class='pagination-page'>...</a>";
                    pagingstr += "</li>";
                }

                for (int p = Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)) - 2; p <= Convert.ToInt32(Math.Ceiling((decimal)Count / CountPerPage)); p++)
                {
                    pagingstr += "<li>";
                    pagingstr += "<a href='#' onclick='return show_report_page(" + (p - 1).ToString() + ")' class='pagination-page" + ((p == PageNum + 1) ? "-active" : "") + "'>" + p.ToString() + "</a>";
                    pagingstr += "</li>";
                }
            }

            return pagingstr;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Search")]
        public async Task<ActionResult> Search()
        {
            //if (!Authentication.CheckUser(this.HttpContext))
            //{
            //    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            //}
            try
            {
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));

                var sSearch = "";

                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                //    var usr = Authentication.GetCurrentUser();
                //    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                //    {
                //        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                //    }

                var pss = new PropertySorter();
                var pdc = TypeDescriptor.GetProperties(item);
                var search_properties = pss.GetSearchProperties(pdc);

                foreach (AdvancedProperty property in search_properties)
                {
                    property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", Lib.AdvancedProperties.DisplayMode.Search));
                }

                item.DefaultReportFilter(false);

                if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                {
                    sSearch = Request.Form["sSearch"];
                }

                sSearch = item.SimpleSearch(sSearch);

                Dictionary<long, ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
                AdvancedProperties properties = null; ;

                if (HttpContextHelper.Current.Session.TryGetValue("Display_" + item.GetType().Name, out var strD))
                {
                    properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                }
                else if (DBProperties != null && DBProperties.Count > 0)
                {
                    //properties = new AdvancedProperties();
                    //var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                    //foreach (AdvancedProperty property in tprops)
                    //{
                    //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                    //    {
                    //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                    //        property.Common.DisplayName = DBField.Name;
                    //        property.Common.PrintName = DBField.PrintName;

                    //        if (DBField.DisplayModes != null
                    //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Simple)
                    //            //&& usr.HasAtLeastOnePermission(DBField.Permission)
                    //            )
                    //        {
                    //            properties.Add(property);
                    //        }
                    //    }
                    //}
                    //properties.Sort();
                }
                else
                {
                    properties = pss.GetProperties(pdc, Authentication.GetCurrentUser());
                }

                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                {
                    boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];

                    ViewData["NewTab"] = boproperties.OpenInNewTab;
                    ViewData["NoLink"] = boproperties.NoLink;
                    ViewData["ActionOnClick"] = boproperties.ActionOnClick;
                }
                else
                {
                    ViewData["NewTab"] = false;
                    ViewData["NoLink"] = false;
                    ViewData["ActionOnClick"] = false;
                }

                long itotal;
                long idisplaytotal;

                var CountPerPage = !string.IsNullOrEmpty(Request.Form["CountPerPage"]) ? Convert.ToInt32(Request.Form["CountPerPage"]) : boproperties.RecordsPerPage;
                var PageNum = Convert.ToInt32(Request.Form["PageNum"]);
                var SortParameters = new List<SortParameter>();

                if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                {
                    foreach (AdvancedProperty property in properties)
                    {
                        if (property.PropertyName == Request.Form["SortCol"])
                        {
                            var PropertyName = property.PropertyName;
                            if (property.Type.BaseType == typeof(ItemBase)
                                || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                )
                            {
                                PropertyName += "Id";
                            }
                            SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                            break;
                        }
                    }
                }

                ViewData["SortCol"] = Request.Form["SortCol"];
                ViewData["SortDir"] = Request.Form["SortDir"];

                var DateStart = DateTimeZone.Now;
                var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, Lib.AdvancedProperties.DisplayMode.Simple, out itotal, out idisplaytotal);
                ViewData["ReportWidget"] = item.PopulateReportWidget(item, sSearch, ControllerContext, ViewData, TempData);
                var TimeSpent = DateTimeZone.Now - DateStart;
                ViewData["ReuquestExecTime"] = TimeSpent.TotalMinutes.ToString("0") + " " + Helpers.T.Str("ShortMinutes", "FrontEnd", "min") + " " + TimeSpent.Seconds + " " + Helpers.T.Str("ShortSecond", "FrontEnd", "sec");

                ViewData["Count"] = idisplaytotal;
                ViewData["CountPerPage"] = CountPerPage;
                ViewData["PageNum"] = PageNum;
                ViewData["BuildPaginng"] = BuildPaginng(idisplaytotal, CountPerPage, PageNum);

                ViewData["DataItems"] = Items;

                ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
                ViewData["Search_Item"] = item;

                ViewData["Search_Properties"] = search_properties;
                ViewData["Properties"] = properties;

                var Data = new Dictionary<string, object>();

                Data["Search_Result"] = await RenderPartialViewToString("~/Views/Report/Search.cshtml", item);

                return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });

                //}
                //return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Print")]

        public async Task<ActionResult> Print()
        {
            //if (!Authentication.CheckUser(this.HttpContext))
            //{
            //    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            //}
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                //    var usr = Authentication.GetCurrentUser();
                //    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                //    {
                //        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                //    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", Lib.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                Dictionary<long, ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
                AdvancedProperties properties = null; ;

                    if (HttpContextHelper.Current.Session.TryGetValue("Display_" + item.GetType().Name, out var strD))
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        //properties = new AdvancedProperties();
                        //var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        //foreach (AdvancedProperty property in tprops)
                        //{
                        //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                        //    {
                        //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                        //        property.Common.DisplayName = DBField.Name;
                        //        property.Common.PrintName = DBField.PrintName;

                        //        if (DBField.DisplayModes != null
                        //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Print)
                        //            //&& usr.HasAtLeastOnePermission(DBField.Permission)
                        //            )
                        //        {
                        //            properties.Add(property);
                        //        }
                        //    }
                        //}
                        //properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                        if (boproperties != null)
                        {
                            ViewData["Show_Header"] = !boproperties.HideReportHeader;
                        }
                    //}

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var DateStart = DateTimeZone.Now;
                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, Lib.AdvancedProperties.DisplayMode.Simple, out itotal, out idisplaytotal);
                    ViewData["ReportWidget"] = item.PopulateReportWidget(item, sSearch, ControllerContext, ViewData, TempData);
                    var TimeSpent = DateTimeZone.Now - DateStart;
                    ViewData["ReuquestExecTime"] = TimeSpent.TotalMinutes.ToString("0") + " " + Helpers.T.Str("ShortMinutes", "FrontEnd", "min") + " " + TimeSpent.Seconds + " " + Helpers.T.Str("ShortSecond", "FrontEnd", "sec");

                    ViewData["DataItems"] = Items;

                    ViewData["Grid_Type"] = item.GetType().AssemblyQualifiedName;
                    ViewData["Search_Item"] = item;
                    ViewData["Search_Properties"] = search_properties; 

                    ViewData["Properties"] = properties;
                    ViewData["Styles"] = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("PrintStylesPart"),  "common.css"));
                    ViewData["Styles"] += System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("PrintStylesPart"),  "report.css"));

                    var Data = new Dictionary<string, object>();

                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName,
                    //                                                           null);

                    //    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    //    viewResult.View.Render(viewContext, sw);

                    //    Data["Print_Result"] = sw.GetStringBuilder().ToString();
                    //}

                    Data["Print_Result"] = await RenderPartialViewToString("~/Views/Report/Print.cshtml", item);


                    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
                }
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ExportCSV")]

        public async Task<FileResult> ExportCSV()
        {
            //var filedownload = new HttpCookie("fileDownload")
            //{
            //    Expires = DateTimeZone.Now.AddDays(1),
            //    Value = "true"
            //};
            //Response.Cookies.Add(filedownload);
            //var path = new HttpCookie("path")
            //{
            //    Expires = DateTimeZone.Now.AddDays(1),
            //    Value = "/"
            //};
            //Response.Cookies.Add(path);
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return File(Encoding.UTF8.GetBytes("Authentification Error"), "application/vnd.ms-excel", "Error.csv");
            }
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                var usr = Authentication.GetCurrentUser();

                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                //    var usr = Authentication.GetCurrentUser();
                //    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                //    {
                //        return File("", "application/vnd.ms-excel");
                //    }

                var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", Lib.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                Dictionary<long, ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
                AdvancedProperties properties = null; ;

                    if (HttpContextHelper.Current.Session.TryGetValue("Display_" + item.GetType().Name, out var strD))
                    {
                        properties = pss.GetAvailableProperties(pdc, usr, JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        //properties = new AdvancedProperties();
                        //var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        //foreach (AdvancedProperty property in tprops)
                        //{
                        //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                        //    {
                        //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                        //        property.Common.DisplayName = DBField.Name;
                        //        property.Common.PrintName = DBField.PrintName;

                        //        if (DBField.DisplayModes != null
                        //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Print)
                        //            && usr.HasAtLeastOnePermission(DBField.Permission)
                        //            && !HttpContextHelper.Current.Session.TryGetValue("Print_" + item.GetType().Name, out strD))
                        //        {
                        //            properties.Add(property);
                        //        }
                        //    }
                        //}
                        //properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var DateStart = DateTimeZone.Now;
                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, Lib.AdvancedProperties.DisplayMode.Excell, out itotal, out idisplaytotal);
                    ViewData["ReportWidget"] = item.PopulateReportWidget(item, sSearch, ControllerContext, ViewData, TempData);
                    var TimeSpent = DateTimeZone.Now - DateStart;
                    ViewData["ReuquestExecTime"] = TimeSpent.TotalMinutes.ToString("0") + " " + Helpers.T.Str("ShortMinutes", "FrontEnd", "min") + " " + TimeSpent.Seconds + " " + Helpers.T.Str("ShortSecond", "FrontEnd", "sec");

                    return File(CSVExportHelper.ExportCSV(Items, properties), "application/vnd.ms-excel", ViewData["Report_Name"] + ".csv");
                //}
                return File(Encoding.UTF8.GetBytes("No Access"), "application/vnd.ms-excel", "Error.csv");
            }
            catch (Exception ex)
            {
                return File(Encoding.UTF8.GetBytes("Error:" + ex.ToString()), "application/vnd.ms-excel", "Error.csv");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("ExportExcell")]
        public FileResult ExportExcell()
        {
            //var filedownload = new HttpCookie("fileDownload")
            //{
            //    Expires = DateTimeZone.Now.AddDays(1),
            //    Value = "true"
            //};
            //Response.Cookies.Add(filedownload);
            //var path = new HttpCookie("path")
            //{
            //    Expires = DateTimeZone.Now.AddDays(1),
            //    Value = "/"
            //};
            //Response.Cookies.Add(path);
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return File(Encoding.UTF8.GetBytes("Authentification Error"), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }
            try
            {
                var sSearch = "";
                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Request.Form["bo_type"], true));
                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                //    var usr = Authentication.GetCurrentUser();
                //    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                //    {
                //        return File("", "application/ms-excel");
                //    }

                    var pss = new PropertySorter();
                    var pdc = TypeDescriptor.GetProperties(item);
                    var search_properties = pss.GetSearchProperties(pdc, Authentication.GetCurrentUser());

                    foreach (AdvancedProperty property in search_properties)
                    {
                        property.PropertyDescriptor.SetValue(item, property.GetDataProcessor().GetValue(property, "", Lib.AdvancedProperties.DisplayMode.PrintSearch));
                    }

                    item.DefaultReportFilter(false);

                    if (!string.IsNullOrEmpty(Request.Form["sSearch"]))
                    {
                        sSearch = Request.Form["sSearch"];
                    }

                    sSearch = item.SimpleSearch(sSearch);

                Dictionary<long, ItemBase> DBProperties = null;//Field.LoadByPage(item.GetType().FullName);
                AdvancedProperties properties = null; ;

                    if (HttpContextHelper.Current.Session.TryGetValue("Display_" + item.GetType().Name, out var strD))
                    {
                        properties = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser(), JsonConvert.DeserializeObject<List<string>>(Encoding.Default.GetString(strD)));
                    }
                    else if (DBProperties != null && DBProperties.Count > 0)
                    {
                        //properties = new AdvancedProperties();
                        //var tprops = pss.GetAvailableProperties(pdc, Authentication.GetCurrentUser());
                        //foreach (AdvancedProperty property in tprops)
                        //{
                        //    if (DBProperties.Values.Any(f => f.FieldName == property.PropertyName))
                        //    {
                        //        var DBField = DBProperties.Values.FirstOrDefault(f => f.FieldName == property.PropertyName);
                        //        property.Common.DisplayName = DBField.Name;
                        //        property.Common.PrintName = DBField.PrintName;

                        //        if (DBField.DisplayModes != null
                        //            && DBField.DisplayModes.Values.Any(dm => dm == SecurityCRMLib.BusinessObjects.DisplayMode.Print)
                        //            //&& usr.HasAtLeastOnePermission(DBField.Permission)
                        //            && !HttpContextHelper.Current.Session.TryGetValue("Print_" + item.GetType().Name, out strD))
                        //        {
                        //            properties.Add(property);
                        //        }
                        //    }
                        //}
                        //properties.Sort();
                    }
                    else
                    {
                        properties = pss.GetPrintProperties(pdc, Authentication.GetCurrentUser());
                    }

                    BoAttribute boproperties = null;
                    if (item.GetType().GetCustomAttributes(typeof(BoAttribute), true).Length > 0)
                    {
                        boproperties = (BoAttribute)item.GetType().GetCustomAttributes(typeof(BoAttribute), true)[0];
                        if (boproperties != null && !string.IsNullOrEmpty(boproperties.DisplayName))
                        {
                            ViewData["Report_Name"] = boproperties.DisplayName;
                        }
                    }

                    long itotal;
                    long idisplaytotal;

                    var CountPerPage = 0;
                    var PageNum = 0;
                    var SortParameters = new List<SortParameter>();

                    if (!string.IsNullOrEmpty(Request.Form["SortCol"]))
                    {
                        foreach (AdvancedProperty property in properties)
                        {
                            if (property.PropertyName == Request.Form["SortCol"])
                            {
                                var PropertyName = property.PropertyName;
                                if (property.Type.BaseType == typeof(ItemBase)
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType == typeof(ItemBase))
                                    || (property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType != null && property.Type.BaseType.BaseType.BaseType == typeof(ItemBase))
                                    )
                                {
                                    PropertyName += "Id";
                                }
                                SortParameters.Add(new SortParameter() { Direction = Request.Form["SortDir"], Field = PropertyName });
                                break;
                            }
                        }
                    }

                    var DateStart = DateTimeZone.Now;
                    var Items = item.PopulateReport(null, item, PageNum * CountPerPage, CountPerPage, sSearch, SortParameters, null, Lib.AdvancedProperties.DisplayMode.Excell, out itotal, out idisplaytotal);
                    var additionalInfo = item.getAdditionalPrintInfo(item);
                    var TimeSpent = DateTimeZone.Now - DateStart;
                    ViewData["ReuquestExecTime"] = TimeSpent.TotalMinutes.ToString("0") + " " + Helpers.T.Str("ShortMinutes", "FrontEnd", "min") + " " + TimeSpent.Seconds + " " + Helpers.T.Str("ShortSecond", "FrontEnd", "sec");

                    return File(ExcelExportHelper.ExportExcel(Items, properties, ViewData["Report_Name"].ToString(), false, additionalInfo), ExcelExportHelper.ExcelContentType, ViewData["Report_Name"] + ".xlsx");
                //}
                return File(Encoding.UTF8.GetBytes("No Access"), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return File(Encoding.UTF8.GetBytes("Error:" + ex.ToString()), ExcelExportHelper.ExcelContentType, "Error.xlsx");
            }

        }
    }
}
