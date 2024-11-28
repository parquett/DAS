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

using Lib.AdvancedProperties;
using System.ComponentModel;
using Lib.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SecurityCRM.Controllers
{
    public class MultySelectController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public MultySelectController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        //
        // GET: /MultySelect/

        public ActionResult Options(string Namespace, string Param)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));
            var Items = item.PopulateAutocomplete(Param,Request.Query["term"]);
            ViewData["Param"] = Param;
            ViewData["Values"] = !string.IsNullOrEmpty(Request.Form["values"])?Request.Form["values"].ToString().Split(','):null;
            ViewData["AllowDefault"] = false;
            return View("Options", Items);
        }
    }
}
