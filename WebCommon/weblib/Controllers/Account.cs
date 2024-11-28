using weblib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Controllers
{

    using Lib.BusinessObjects;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Collections.Generic;
    using Lib.Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;
    using lib;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using ApiContracts.Enums;
    using System.Threading.Tasks;
    using SecurityCRM.Helpers;
    using SecurityCRM.ApiContracts.DTO.User;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using SecurityCRMLib.Security;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using Nest;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class AccountBaseController : BaseController
    {

        private readonly string _pepper = Environment.GetEnvironmentVariable("PasswordHashExamplePepper");
        private readonly int _iteration = 3;

        public AccountBaseController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
            SessionHelper<int>.Push("IsSafary", 0);
            SessionHelper<int>.Push("IsChrome", 0);
            SessionHelper<int>.Push("IsFF", 0);
            SessionHelper<int>.Push("IsMacSafary", 0);
            SessionHelper<int>.Push("IsMacChrome", 0);
            SessionHelper<int>.Push("IsIE10", 0);
            SessionHelper<int>.Push("IsIE11", 0);
            SessionHelper<int>.Push("IsIE9", 0);
            SessionHelper<int>.Push("IsAndroidFF", 0);
            SessionHelper<int>.Push("IsAndroidNative", 0);
            SessionHelper<int>.Push("IsIPad", 0);
            SessionHelper<int>.Push("IsMacFF", 0);
            SessionHelper<int>.Push("IsIEOld", 0);
            SessionHelper<string>.Push("Browser_specific", "");

            var ua = HttpContextHelper.Current.Request.Headers["User-Agent"].ToString();

            if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("safari") != -1 && ua.ToLower().IndexOf("chrome") == -1)
            {
                SessionHelper<int>.Push("IsSafary", 1); ;
                SessionHelper<string>.Push("Browser_specific", "win_safari");
            }
            else if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("chrome") != -1)
            {
                SessionHelper<int>.Push("IsChrome", 1); ;
                SessionHelper<string>.Push("Browser_specific", "win_chrome");
            }
            else if (ua.ToLower().IndexOf("windows") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                SessionHelper<int>.Push("IsFF", 1); ;
                SessionHelper<string>.Push("Browser_specific", "win_firefox");
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("safari") != -1 && ua.ToLower().IndexOf("chrome") == -1)
            {
                SessionHelper<int>.Push("IsMacSafary", 1); ;
                SessionHelper<string>.Push("Browser_specific", "mac_safari");
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("chrome") != -1)
            {
                SessionHelper<int>.Push("IsMacChrome", 1); ;
                SessionHelper<string>.Push("Browser_specific", "mac_chrome");
            }
            else if (ua.ToLower().IndexOf("rv:11.0") != -1)
            {
                SessionHelper<int>.Push("IsIE11", 1); ;
                SessionHelper<string>.Push("Browser_specific", "ie11");
            }
            else if (ua.ToLower().IndexOf("msie 10") != -1)
            {
                SessionHelper<int>.Push("IsIE10", 1); ;
                SessionHelper<string>.Push("Browser_specific", "ie10");
            }
            else if (ua.ToLower().IndexOf("msie 9") != -1)
            {
                SessionHelper<int>.Push("IsIE9", 1); ;
                SessionHelper<string>.Push("Browser_specific", "ie9");
            }
            else if (ua.ToLower().IndexOf("msie") != -1)
            {
                SessionHelper<int>.Push("IsIEOld", 1);
                SessionHelper<string>.Push("Browser_specific", "ieold");
            }
            else if (ua.ToLower().IndexOf("android") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                SessionHelper<int>.Push("IsAndroidFF", 1); ;
                SessionHelper<string>.Push("Browser_specific", "android_firefox");
            }
            else if (ua.ToLower().IndexOf("android") != -1)
            {
                SessionHelper<int>.Push("IsAndroidNative", 1); ;
                SessionHelper<string>.Push("Browser_specific", "android");
            }
            else if (ua.ToLower().IndexOf("ipad") != -1)
            {
                SessionHelper<int>.Push("'IsIPad'", 1); ;
                SessionHelper<string>.Push("Browser_specific", "ipad");
            }
            else if (ua.ToLower().IndexOf("macintosh") != -1 && ua.ToLower().IndexOf("firefox") != -1)
            {
                SessionHelper<int>.Push("IsMacFF", 1); ;
                SessionHelper<string>.Push("Browser_specific", "mac_firefox");
            }
        }

        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";

            var model = new LoginModel
            {
                Login = new TextboxModel() { Name = "Login", Type = TextboxType.Text, ValidationType = Lib.AdvancedProperties.ValidationTypes.Required },
                Password = new TextboxModel() { Name = "Password", Type = TextboxType.Password, ValidationType = Lib.AdvancedProperties.ValidationTypes.Required, OnType = "submit_on_enter(this,event)" }
            };

            return this.View(model);
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

        protected async Task<ActionResult> Login(User user, string returnUrl)
        {
            try
            {
                var conn = DataBase.ConnectionFromContext();
                var cmd = new SqlCommand("SELECT * FROM [User] u WHERE u.Login = @Login", conn);
                cmd.Parameters.AddWithValue("@Login", user.Login);
                using (var rdr = cmd.ExecuteReader(System.Data.CommandBehavior.SingleResult))
                {
                    while (rdr.Read())
                    {
                        var storedUser = new User();
                        storedUser.Login = rdr["Login"].ToString();
                        storedUser.PasswordSalt = rdr["PasswordSalt"].ToString();
                        storedUser.PasswordHash = rdr["PasswordHash"].ToString();

                        var passwordHash = PasswordHasher.ComputeHash(user.Password, storedUser.PasswordSalt, _pepper, _iteration);

                        if (storedUser.PasswordHash == passwordHash)
                        {
                            await Lib.Tools.Security.Authentication.DoAuthorization(storedUser, null, module: Modulesenum.SMI);

                            if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/")
                                returnUrl = Lib.Tools.Utils.URLHelper.GetUrl("");
                            if (!returnUrl.Contains("http"))
                                returnUrl = Lib.Tools.Utils.URLHelper.GetUrl(returnUrl);

                            return Json(new RequestResult() { Message = "Authentication successful", Result = RequestResultType.Success, RedirectURL = returnUrl });
                        }
                    }

                    var errorFields = new List<string>() { "input[name=Login]", "input[name=Password]" };
                    return Json(new RequestResult() { Message = "Acest utilizator nu exista", Result = RequestResultType.Fail, ErrorFields = errorFields });
                }
            }

            catch (Exception ex)
            {
                return Json(new RequestResult() { Message = "An error occurred", Result = RequestResultType.Fail });
            }
        }

        //protected async Task<ActionResult> Login(User user, string returnUrl)
        //{
        //    var DeviceGuidId = Guid.NewGuid();
        //    var usr = await SecurityCRMAuthApiHelper.PostRequestAsync<AuthenticatedResponse>("Auth/login", HttpContext, new LoginResource(user.Login, user.Password, DeviceGuidId));
        //    if (usr is null)
        //    {


        //        var errorFields = new List<string>()
        //        {
        //            "input[name=Login]",
        //            "input[name=Password]"
        //        };
        //        return this.Json(new RequestResult() { Message = "Acest utilizator nu exista", Result = RequestResultType.Fail, ErrorFields = errorFields });
        //    }

        //    await Lib.Tools.Security.Authentication.DoAuthorization(usr, null, module: Modulesenum.SMI);

        //    if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/")
        //        returnUrl = Lib.Tools.Utils.URLHelper.GetUrl("SystemManagement");
        //    if (!returnUrl.Contains("http"))
        //        returnUrl = Lib.Tools.Utils.URLHelper.GetUrl(returnUrl);
        //    HttpContext.Response.Cookies.Append("refreshToken", usr.RefreshToken, new CookieOptions() { SameSite = SameSiteMode.Unspecified });
        //    HttpContext.Response.Cookies.Append("DeviceId", DeviceGuidId.ToString(), new CookieOptions() { SameSite = SameSiteMode.Unspecified });
        //    return this.Json(new RequestResult() { Message = "Acest utilizator nu exista", Result = RequestResultType.Success, RedirectURL = returnUrl });
        //}


        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected ActionResult CPLogin(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";

            ViewData["LoginFail"] = null;
            return this.View();
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //protected async Task<ActionResult> CPLogin(User user, string returnUrl)
        //{
        //    if (await Lib.Tools.Security.Authentication.DoAuthorization(user, null, module:Modulesenum.ControlPanel))
        //    {
        //        if (string.IsNullOrEmpty(returnUrl))
        //            returnUrl = Lib.Tools.Utils.URLHelper.GetUrl("ControlPanel");
        //        return this.Redirect(returnUrl);
        //    }
        //    ViewData["LoginFail"] = true;

        //    return this.View();
        //}


        /// <summary>
        /// The login.
        /// </summary>
        /// <param name="returnUrl">
        /// The return url.
        /// </param>
        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        protected ActionResult SMILogin(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            this.ViewBag.Script = "login";

            ViewData["LoginFail"] = null;
            return this.View();
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //protected async Task<ActionResult> SMILogin(User user, string returnUrl)
        //{
        //    if (await Lib.Tools.Security.Authentication.DoAuthorization(user, null, module: Modulesenum.SMI))
        //    {
        //        if (string.IsNullOrEmpty(returnUrl))
        //            returnUrl = Lib.Tools.Utils.URLHelper.GetUrl("SystemManagement");
        //        return this.Redirect(returnUrl);
        //    }
        //    ViewData["LoginFail"] = true;

        //    return this.View();
        //}

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult LogOff()
        {
            HttpContext.Response.Cookies.Delete("DeviceId");
            HttpContext.Response.Cookies.Delete("refreshToken");
            Lib.Tools.Security.Authentication.LogOff();
            return this.RedirectToAction("Login", "Account");
        }

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult CPLogOff()
        {
            Lib.Tools.Security.Authentication.LogOff();
            return this.RedirectToAction("CPLogin", "Account");
        }

        /// <returns>
        /// The <see cref="ActionResult"/>.
        /// </returns>
        public ActionResult SMILogOff()
        {
            SecurityCRMAuthApiHelper.Logout();
            return this.RedirectToAction("SMILogin", "Account");
        }
    }
}
