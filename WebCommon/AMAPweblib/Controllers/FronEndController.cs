using SecurityCRMweblib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Account.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Controllers
{
    using System.Linq;
    using System.Collections.Generic;

    using Lib.BusinessObjects;
    using Lib.Tools.Security;
    using Lib.Tools.Utils;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;

    using SecurityCRMLib.BusinessObjects;
    using System;
    using SecurityCRMLib;
    using Lib.Tools.BO;
    using Microsoft.AspNetCore.Http;
    using Lib.Helpers;
    using lib;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The account controller.
    /// </summary>
    [AuthAction]
    [DataForMasterPageAttribute]
    public class FrontEndController : Weblib.Controllers.FrontEndController
    {
        public FrontEndController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
            if (SecurityCRMLib.BusinessObjects.Person.Current()==null && Authentication.GetCurrentUser() != null)
            {
                SecurityCRMLib.BusinessObjects.Person.AddPersonInfo(Authentication.GetCurrentUser());
            }
            ViewBag.Title = "SecurityCRM";

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
    }
}
