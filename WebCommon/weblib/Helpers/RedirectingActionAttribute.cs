using lib;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weblib;

namespace Weblib.Helpers
{
    public class AuthActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Authentication.CheckUser())
            {
                filterContext.Result = new RedirectResult(Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + HttpContextHelper.Current.Request.GetEncodedPathAndQuery());
                return;
            }    

            base.OnActionExecuting(filterContext);           
        }
    }
}
