using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;

namespace SecurityCRM.Controllers
{
    public class HelpController : Controller
    {
        //
        // GET: /Help/

        public ActionResult Index()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            return View();
        }

    }
}
