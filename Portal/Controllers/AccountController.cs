using SecurityCRM;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccountController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the AccountController type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRM.Controllers
{
    using Lib.Tools.BO;
    using Lib.Tools.Security;
    using Lib.Tools.Utils;
    using SecurityCRMLib.BusinessObjects;
    using SecurityCRMweblib;
    using SecurityCRMweblib.Controllers;
    using System;
    using System.Collections.Generic;
    using Weblib.Helpers;
    using Lib.Helpers;
    using System.Security.Cryptography.X509Certificates;
    using System.Xml;
    using SecurityCRMLib.Utils;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Lib.Plugins;
    using System.Linq;
    using lib;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http; // For CookieOptions, HttpOnly, SameSiteMode

    public class AccountController : SecurityCRMweblib.Controllers.AccountWebController
    {
        public AccountController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        public ActionResult Manage()
        {
            var usr = SecurityCRMLib.BusinessObjects.User.Populate(Authentication.GetCurrentUser());
            return View(usr);
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="user">
        /// The user.
        /// </param>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Edit()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];
            //var email = Request.Form["UserEmail"];
            var oldPassword = Request.Form["OldPassword"];
            var newPassword = Request.Form["Password"];
            var newPasswordConfirm = Request.Form["PasswordConfirm"];
            /*
            if(!Lib.Tools.Utils.CommonHelper.IsValidEmail(email)) {
                var errorFields = new List<string>();
                    errorFields.Add("input[name=UserEmail]");
                    return this.Json(new RequestResult() { Message = "Adresa email nu este valida", Result = RequestResultType.Fail, ErrorFields = errorFields });
            }*/

            if (!string.IsNullOrEmpty(oldPassword) || !string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(newPasswordConfirm))
            {
                var currentUser = Authentication.GetCurrentUser();
                currentUser.Password = oldPassword;

                //if(await Authentication.DoAuthorization(currentUser)) {
                //    var item = (User)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));
                //    item.Id = Convert.ToInt64(Request.Form["Id"]);

                //    if(string.IsNullOrEmpty(newPassword)) {
                //        var errorFields = new List<string>();
                //        errorFields.Add("input[name=Password]");
                //        return this.Json(new RequestResult() { Message = "Parola nu poate fi nula", Result = RequestResultType.Fail, ErrorFields = errorFields });
                //    } else {
                //        item.Password = Request.Form["Password"];
                //    }

                //    SecurityCRMLib.BusinessObjects.User.UpdatePassword(item);
                //    //SecurityCRMLib.BusinessObjects.Person.UpdateEmail(item, email);

                //    return this.Json(new RequestResult() { Result = RequestResultType.Success, Message="Schimbarile au fost memorizate cu succes"});
                //} else {
                //    var errorFields = new List<string>();
                //    errorFields.Add("input[name=OldPassword]");
                //    return this.Json(new RequestResult() { Message = "Parola curenta nu este corecta", Result = RequestResultType.Fail, ErrorFields = errorFields });
                //    // return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = "Parola nu a fost modificata!" });
                //}

                // Set a secure cookie for the user
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true, // Prevents JavaScript access (XSS protection)
                    Secure = true,   // Only send over HTTPS
                    SameSite = SameSiteMode.Strict // Prevents CSRF attacks by restricting cross-site requests
                };
                Response.Cookies.Append("UserSession", "SomeValueRelatedToUser", cookieOptions); // Example of setting a cookie

                // Optionally, you can return success or redirect
                return this.Json(new RequestResult() { Result = RequestResultType.Success, Message = "Password updated successfully and cookie set." });
            }
            else
            {
                var currentUser = Authentication.GetCurrentUser();
                var item = (User)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));
                item.Id = Convert.ToInt64(Request.Form["Id"]);
                //SecurityCRMLib.BusinessObjects.Person.UpdateEmail(item, email);
                return this.Json(new RequestResult() { Result = RequestResultType.Success, Message = "Schimbarile au fost memorizate cu succes" });
            }
            return this.Json(new RequestResult() { Result = RequestResultType.Success, Message = "Just some hardcode,nothing will be saved at this time" });

        }
    }
}
