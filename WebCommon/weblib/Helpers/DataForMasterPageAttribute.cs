using SecurityCRMLib.BusinessObjects;
using lib;
using Lib.BusinessObjects.Translations;
using Lib.Helpers;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weblib;

namespace Weblib.Helpers
{
    public class DataForMasterPageAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = HttpContextHelper.Current;
            Language CurrentLanguage = null;

            if (string.IsNullOrEmpty(context.Session.GetString(SessionItems.Language)))
            {
                CurrentLanguage = Language.Romanian;
                //SessionHelper<Language>.Push(SessionItems.Language, CurrentLanguage);
                var str = JsonConvert.SerializeObject(CurrentLanguage);
                context.Session.SetString(SessionItems.Language, str);
            }

            CultureHelper.Language = CurrentLanguage;

            Controller controller = filterContext.Controller as Controller;

            if (controller != null && filterContext.HttpContext.Request.Method.ToUpper()=="GET")
            {
                //var Languages = (new Language()).Populate();
                //controller.ViewData["Languages"] = Languages;
            }
        }
    }
}
