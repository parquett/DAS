// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidationHelperController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the ValidationHelperController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Galex.Controllers
{
    using Lib.BusinessObjects;
    using Lib.Tools.Security;
    using Lib.Tools.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using Weblib.Controllers;
    using Weblib.Helpers;
    using Lib.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The ValidationHelper controller.
    /// </summary>
    public class ValidationHelperController : BaseController
    {
        public ValidationHelperController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        [HttpPost]
        public ActionResult ValidateUserName()
        {
            if (!Authentication.CheckUser(this.HttpContext)) //TBD
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }
            if (!Lib.Tools.Security.Authentication.GetCurrentUser().HasPermissions((long)SecurityCRM.ApiContracts.Enums.Permissions.CPAccess | (long)SecurityCRM.ApiContracts.Enums.Permissions.SMIAccess))
            {
                return new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + Request.GetEncodedPathAndQuery());
            }

            var Login = Request.Form["Login"];
            var Userid = Convert.ToInt64(Request.Form["Userid"]);

            var user = new User() { Login = Login};
            
            user = (User)user.PopulateOne(user);
            if (user == null || (user != null && (Userid != 0 || user.Id == Userid)))
                return this.Json(new RequestResult() { Result = RequestResultType.Fail });

            return this.Json(new RequestResult() { Result = RequestResultType.Success, Message = "Acest nume este utilizat deja" });
        }    
    }
}
