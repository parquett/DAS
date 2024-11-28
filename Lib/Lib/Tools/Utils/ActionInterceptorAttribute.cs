using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Lib.Tools.Utils
{
    public class ActionInterceptorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Session.TryGetValue("token", out var token);
            //context.HttpContext.Request.Headers.
            context.HttpContext.Request.Headers.Authorization = $"Bearer {token.ToString()}";
        }
    }
}
