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
    using Lib.Tools.Utils;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Newtonsoft.Json;
    using SecurityCRMLib.BusinessObjects;
    using Microsoft.AspNetCore.Http;
    using System.Text;
    using System;
    using Lib.Helpers;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.ViewEngines;
    using Microsoft.AspNetCore.Mvc.Rendering;

    /// <summary>
    /// The account controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected ICompositeViewEngine _viewEngine;

        public BaseController(ICompositeViewEngine viewEngine)
        {
            _viewEngine = viewEngine;
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //Response.Redirect("http://SecurityCRM.e-agricultura.md/");

            base.OnActionExecuted(filterContext);

            DataBase.CloseConnection();
        }
        public void UpdateCookiesReffreshTokens(object param)
        {
            HttpContext.Response.Cookies.Delete("refreshToken");
            HttpContext.Response.Cookies.Append("refreshToken", param.ToString());
        }
        protected async Task<string> RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                ViewEngineResult viewResult =
                    _viewEngine.GetView("~/", viewName, false);

                ViewContext viewContext = new ViewContext(
                    ControllerContext,
                    viewResult.View,
                    ViewData,
                    TempData,
                    writer,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);

                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
