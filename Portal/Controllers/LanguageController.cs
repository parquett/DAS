using Lib.Helpers;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Weblib.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SecurityCRM;
using Microsoft.AspNetCore.Http;
using lib;
using DocumentFormat.OpenXml.Wordprocessing;
using Lib.BusinessObjects.Translations;

namespace MedProject.Controllers
{
    [Route("Language")]
    public class LanguageController : Controller
    {
        //
        // GET: /Language/
        [Route("Change/{LanguageId}")]
        public ActionResult Change(long LanguageId)
        {
            var context = HttpContextHelper.Current;
            var Language = new Lib.BusinessObjects.Translations.Language(LanguageId);
            Language= (Lib.BusinessObjects.Translations.Language)Language.PopulateOne(Language);
            var str = JsonConvert.SerializeObject(Language);
            context.Session.SetString(SessionItems.Language, str);
            //SessionHelper<string>.Push(SessionItems.Language, str);
            CultureHelper.Language = Language;
            return Json(new RequestResult() { Result = RequestResultType.Reload });
        }

    }
}
