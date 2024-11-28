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
    public class AutoCompleteController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public AutoCompleteController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        //
        // GET: /AutoComplete/
        [HttpGet]
        [Route("AutoComplete/{Namespace?}")]
        public ActionResult List(string Namespace)
        {
            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.Split('.')[0], true));

            if (item.CheckAutocompleteSecurity())
            {
                if (!Authentication.CheckUser(this.HttpContext))
                {
                    return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
                }
            }

            var Items = item.PopulateAutocomplete(Request.Query["cond"], Request.Query["term"]);

            var Model = Request.Query["model"];
            ViewData["Model"] = Model;
            ViewData["Param"] = Request.Query["cond"];
            return View(!string.IsNullOrEmpty(Model)?item.AutocompleteControl(): "Search", Items);
        }
    }
}
