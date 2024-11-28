using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Globalization;

using Lib.Tools.BO;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using Weblib.Helpers;
using Lib.BusinessObjects;
using SecurityCRMLib.BusinessObjects;

using Lib.AdvancedProperties;
using System.ComponentModel;
using Weblib.Models.Common;
using Lib.Helpers;
using Lib.Models.Common;
using SecurityCRMLib.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
/*using SecurityCRM.Models.Print;*/

namespace SecurityCRM.Controllers
{
    [Route("DocControl")]
    public class DocControlController : SecurityCRMweblib.Controllers.PageObjectController
    {
        public DocControlController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        //
        // GET: /LabControl/

        [HttpGet]
        [Route("{Model}/{Id?}/{additional?}")]
        public ActionResult Edit(string Model, string Id = "", string additional = "")
        {
            var item = (ItemBase)Activator.CreateInstance(Type.GetType("SecurityCRM.Models.Objects." + Model, true));
            var usr = Authentication.GetCurrentUser();
            if (!Authorization.hasPageAccess(usr, item))
            {
                return Redirect(URLHelper.GetUrl("Error/AccessDenied"));
            }

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
            }


            if (!string.IsNullOrEmpty(Id))
            {
                item.Id = Convert.ToInt64(Id);

                item = item.PopulateFrontEnd(additional, (ItemBase)item);
            }

            var PageTitle = item.LoadPageTitle();
            if (!string.IsNullOrEmpty(PageTitle))
                ViewBag.Title = PageTitle;

            ViewData["Breadcrumbs"] = item.LoadBreadcrumbs();
            ViewData["QuickLinks"] = item.LoadQuickLinks();
            ViewData["ReportMenu"] = item.LoadContextReports();
            if (boproperties != null && !string.IsNullOrEmpty(boproperties.CustomFormTag))
            {
                General.TraceWarn("Elements ___ from here ");

            }
            return View(Model, item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Save")]
        public ActionResult Save()
        {
            //if (!Authentication.CheckUser(this.HttpContext))
            //{
            //    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            //}
            try
            {
                var Namespace = Request.Form["Namespace"];
                var Object = Request.Form["Object"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.ToString().Split('.')[0], true));
                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                    //var usr = Authentication.GetCurrentUser();
                    //if (!Authorization.hasPageAccess(MenuItems, usr, item))
                    //{
                    //    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                    //}
                    item.Id = Convert.ToInt64(Request.Form["Id"]);
                    item.CollectFromForm();
                    return this.Json(item.SaveForm());
                //}
                //return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [HttpPost]
        [Route("Delete")]

        public ActionResult Delete()
        {
            //if (!Authentication.CheckUser(this.HttpContext))
            //{
            //    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            //}
            try
            {
                var Namespace = Request.Form["Namespace"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));
                //var MenuItems = (Dictionary<long, MenuGroup>)ViewData["MainMenu"];
                //if (MenuItems != null)
                //{
                //    var usr = Authentication.GetCurrentUser();
                //    if (!Authorization.hasPageAccess(MenuItems, usr, item))
                //    {
                //        return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                //    }
                    item.Id = Convert.ToInt64(Request.Form["Id"]);
                    var deletearray = new Dictionary<long, ItemBase>();
                    deletearray.Add(item.Id, item);
                    item.Delete(deletearray);

                    return this.Json(new RequestResult() { Result = RequestResultType.Reload, Message = "Success", RedirectURL = URLHelper.GetUrl("") });
                //}
                //return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
    }
}
