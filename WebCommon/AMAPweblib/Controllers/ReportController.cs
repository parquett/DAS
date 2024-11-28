using SecurityCRMweblib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportController.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Account type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMweblib.Controllers
{

    using Lib.BusinessObjects;

    using Weblib.Helpers;
    using Weblib.Models;
    using Weblib.Models.Common;
    using Weblib.Models.Common.Enums;
    using System.Collections.Generic;
    using SecurityCRMLib.BusinessObjects;
    using Lib.Tools.Security;
    using System.Linq;
    using System;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Lib.Helpers;
    using lib;
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class ReportObjectController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public ReportObjectController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
            var usr = Authentication.GetCurrentUser();
            if (usr != null)
            {
            }
        }
    }
}
