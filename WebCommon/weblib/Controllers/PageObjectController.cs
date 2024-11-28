// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageObjectController.cs" company="SecurityCRM">
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
    using Microsoft.AspNetCore.Mvc.ViewEngines;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class PageObjectController : Weblib.Controllers.FrontEndController
    {
        public PageObjectController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }
    }
}
