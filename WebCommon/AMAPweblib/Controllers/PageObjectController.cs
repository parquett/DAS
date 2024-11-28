using SecurityCRMweblib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageObjectController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Controllers
{
    using System;
    using System.Linq;
    using Lib.BusinessObjects;
    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Collections.Generic;
    using SecurityCRMLib.BusinessObjects;
    using Lib.Tools.Security;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Lib.Helpers;
    using Lib.Tools.Utils;
    using lib;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class PageObjectController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public PageObjectController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
            var usr = Authentication.GetCurrentUser();
            if (usr != null)
            {
            }
        }

    }
}
