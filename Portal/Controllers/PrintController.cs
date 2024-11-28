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
using Lib.Helpers;
using Weblib.Converters;
using Microsoft.AspNetCore.Mvc;
using Lib.Plugins;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SecurityCRM.Controllers
{
    [Route("Print")]
    public class PrintController : SecurityCRMweblib.Controllers.PrintController
    {
        public PrintController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        [HttpPost]
        [Route("Print")]
        public ActionResult Print()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                ViewData["SandboxPrint"] = false;
                ViewData["Styles"] = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("PrintStylesPart"), "common.css"));
                var Namespace = Request.Form["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));

                var Filters = new Dictionary<string, string>();             
                foreach (var postItem in Request.Form.Keys)
                {
                    Filters.Add(postItem, Request.Form[postItem]);
                }
                item.Filters = Filters;
                
                var View = item.LoadReport(_viewEngine, ControllerContext, ViewData, TempData);

                if (!string.IsNullOrEmpty(item.StylesFile) && System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("PrintStylesPart") + item.StylesFile + ".css")))
                {
                    ViewData["Styles"] += System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory() + Config.GetConfigValue("PrintStylesPart") + item.StylesFile + ".css"));
                }

                return this.View(View, item);
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
        [HttpPost]
        [Route("SandboxPrint/{View?}/{Postifx?}")]
        public ActionResult SandboxPrint(string View = "", string Postfix = "")
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                ViewData["SandboxPrint"] = true;
                ViewData["Styles"] = System.IO.File.ReadAllText(PluginManager.MapPath(@"~/Content/Print/common.css"));

                if (!string.IsNullOrEmpty(View))
                {
                    if (System.IO.File.Exists(PluginManager.MapPath(@"~/Content/Print/" + View + ".css")))
                    {
                        ViewData["Styles"] += System.IO.File.ReadAllText(PluginManager.MapPath(@"~/Content/Print/" + View + ".css"));
                    }
                    var titem = new PrintTest();
                    //var viewName = "~/Views/Print/Templates/"+ View + Postfix + ".cshtml";
                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);

                    //    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                    //    viewResult.View.Render(viewContext, sw);

                    //    titem.PrintTemplate = sw.GetStringBuilder().ToString();
                    //}
                    return this.View("Generic", titem);
                }

                var Namespace = Request.Query["Namespace"];

                var item = (PrintBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.ToString().Split('.')[0], true));

                var Filters = new Dictionary<string, string>();
                foreach (var postItem in Request.Query.Keys)
                {
                    Filters.Add(postItem, Request.Query[postItem]);
                }
                item.Filters = Filters;

                View = item.LoadReport(_viewEngine, ControllerContext, ViewData, TempData);

                if (!string.IsNullOrEmpty(item.StylesFile) && System.IO.File.Exists(PluginManager.MapPath(@"~/Content/Print/" + item.StylesFile + ".css")))
                {
                    ViewData["Styles"] += System.IO.File.ReadAllText(PluginManager.MapPath(@"~/Content/Print/" + item.StylesFile + ".css"));
                }

                return this.View(View, item);
            }
            catch (Exception ex)
            {
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
    }
}
