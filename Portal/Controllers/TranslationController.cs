using SecurityCRMLib.BusinessObjects;
using SecurityCRMweblib.Controllers;
using Lib.Helpers;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Tools.BO;
using Lib.Tools.Revisions;
using Lib.AdvancedProperties;
using System.ComponentModel;
using Weblib.Helpers;
using Person = SecurityCRMLib.BusinessObjects.Person;
using User = Lib.BusinessObjects.User;
using Lib.Tools.Controls;
using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using SecurityCRM;
using lib;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Lib.BusinessObjects.Translations;

namespace DOCPortal.Controllers
{
    [Route("Translation")]
    public class TranslationController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public TranslationController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        public long TotalValues { get; set; }
        public string PredefinedFilters { get; set; }

        [HttpGet]
        [Route(@"")]
        public ActionResult Index()
        {
            var Assembly = StaticTranslationAssembly.AdminArea;
            if (!string.IsNullOrEmpty(Request.Query["Assembly"]))
            {
                Assembly = new StaticTranslationAssembly(Convert.ToInt64(Request.Query["Assembly"]));
            }
            LoadData(Assembly.Id);
            ViewData["Tabs"] = new StaticTranslationAssembly().Populate();
            ViewData["Languages"] = new Language().Populate();
            ViewData["PredefinedFilters"] = PredefinedFilters;
            ViewData["Assembly"] = Assembly;
            return View();

        }

        [HttpGet]
        [Route(@"LoadGrid")]
        public  ActionResult LoadGrid()
        {
            var Assembly = StaticTranslationAssembly.AdminArea;
            if (!string.IsNullOrEmpty(Request.Form["Assembly"]))
            {
                Assembly = new StaticTranslationAssembly(Convert.ToInt64(Request.Form["Assembly"]));
            }
            ViewData["Languages"] = new Language().Populate();
            LoadData(Assembly.Id);
            //var viewName = "~/Views/Translation/_translationGrid.cshtml";
            var Data = new Dictionary<string, object>();
            //using (StringWriter sw = new StringWriter())
            //{
            //    var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, null);
            //    var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
            //    viewResult.View.Render(viewContext, sw);
            //    Data.Add("View", sw.GetStringBuilder().ToString());
            //}
            Data.Add("TotalValues", TotalValues);
            return Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });

        }

        [HttpGet]
        [Route(@"LoadRow")]
        public ActionResult LoadRow()
        {
            var currentUser = Authentication.GetCurrentUser();
            if (currentUser != null)
            {
                var Translation = new StaticTranslation();
                if (!string.IsNullOrEmpty(Request.Form["TranslationId"]))
                {
                    Translation.Id = Convert.ToInt64(Request.Form["TranslationId"]);
                    Translation = StaticTranslation.PopulateOne(Translation);
                }
                ViewData["Prefix"] = Convert.ToInt32(Request.Form["Prefix"]);
                ViewData["Languages"] = new Language().Populate();
                return View("_newTranslation", Translation);
            }
            return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Save")]
        public ActionResult Save()
        {
            var currentUser = Authentication.GetCurrentUser();
            if (currentUser != null)
            {

                var item = new StaticTranslation(Convert.ToInt64(Request.Form["TranslationId"]));
                item.Key = Request.Form["Key"];
                item.StaticTranslationAssembly = new StaticTranslationAssembly(Convert.ToInt64(Request.Form["AssemblyId"]));


                if (item.Id > 0)
                {
                    item.Update(item);
                }
                else
                {
                    if (StaticTranslation.TranslationExist(item))
                    {
                        return this.Json(new RequestResult() { Message = SecurityCRM.Helpers.T.Str("Translation already exist", "FrontEnd", "Translation already exist"), Result = RequestResultType.Alert });
                    }
                    item.Insert(item);
                }
                if (!string.IsNullOrEmpty(Request.Form["Languages[]"]) && !string.IsNullOrEmpty(Request.Form["LanguageValues[]"]))
                {
                    item.ClearValues();
                    var Languages = Request.Form["Languages[]"];
                    var Values = Request.Form["LanguageValues[]"];
                    for (var i = 0; i < Languages.ToString().Split(',').Length; i++)
                    {
                        var Value = new StaticTranslationValue();
                        var Language = new Language(Convert.ToInt64(Languages.ToString().Split(',')[i]));
                        Value.StaticTranslation = item;
                        Value.Language = Language;
                        Value.Value = Values.ToString().Split(',')[i].Replace("|", ",");
                        Value.Insert(Value);
                    }
                }
                var Translations = StaticTranslation.Populate();

                item = StaticTranslation.PopulateOne(item);
                ViewData["Prefix"] = Convert.ToInt32(Request.Form["Prefix"]);
                return View("_translation", item);
            }
            return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete()
        {
            var currentUser = Authentication.GetCurrentUser();
            if (currentUser != null)
            {
                var item = new StaticTranslation(Convert.ToInt64(Request.Form["TranslationId"]));

                if (item.Id > 0)
                {
                    var forDelete = new Dictionary<long, ItemBase>();
                    forDelete.Add(item.Id, item);
                    item.Delete(forDelete);
                }
                return Json(new RequestResult() { Result = RequestResultType.Success});

            }
            else
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
        }

        private  Dictionary<long, ItemBase> LoadData(long LatestId = 0, bool bPaging = true)
        {
            var currentUser = Authentication.GetCurrentUser();
            var Assembly = new StaticTranslationAssembly(LatestId);
            int PageNum = 0;
            if (!string.IsNullOrEmpty(Request.Form["PageNum"]) && bPaging)
            {
                PageNum = Convert.ToInt32(Request.Form["PageNum"]);
            }
            var GlobalSearch = "";
            if (!string.IsNullOrEmpty(Request.Form["GlobalSearch"]))
            {
                GlobalSearch = Request.Form["GlobalSearch"];
            }
            var Key = "";
            if (!string.IsNullOrEmpty(Request.Form["Key"]))
            {
                Key = Request.Form["Key"];
            }
            long total = 0;
            int iPagingLen = bPaging ? 10 : -1;
            if (!string.IsNullOrEmpty(Request.Form["CountPerPage"]) && bPaging)
            {
                iPagingLen = Convert.ToInt32(Request.Form["CountPerPage"]);
            }

            int iPagingStart = PageNum * iPagingLen;
            ViewData["Translations"] = StaticTranslation.PopulateWithPaging(null
                          , iPagingStart
                          , iPagingLen
                          , Assembly
                          , GlobalSearch
                          , Key
                          , out total);
            ViewData["Languages"] = new Language().Populate();
            ViewData["GridInfo"] = Assembly.Id.ToString();
            ViewData["displaytotal"] = total;
            ViewData["iPagingStart"] = iPagingStart;
            ViewData["iPagingEnd"] = (iPagingStart + iPagingLen) > total ? total : iPagingStart + iPagingLen;
            ViewData["Count"] = total;
            ViewData["iPagingLen"] = iPagingLen;
            ViewData["GridController"] = "Translation";
            ViewData["GridObject"] = "translation";
            ViewData["GridObjectPlural"] = "translations";
            ViewData["PageNum"] = PageNum;

            TotalValues = total;

            if (bPaging)
            {
                ViewData["BuildPaginng"] = PagingHelper.BuildPaginng("Translation", total, iPagingLen, PageNum);
            }

            return null;
        }

        [HttpGet]
        [Route("DynamicTranslation")]
        public ActionResult DynamicTranslation()
        {
            var BOName = Request.Query["BOName"].ToString();
            var BOId = Convert.ToInt64(Request.Query["BOId"].ToString());
            var PropertyName = Request.Query["PropertyName"].ToString();
            var Original = Request.Query["Original"].ToString();
            var Translations = Translation.PupulateByKey(BOName, BOId, PropertyName);
            ViewData["Languages"] = new Language().Populate();
            ViewData["Translations"] = Translations;
            ViewData["BOName"] = BOName;
            ViewData["BOId"] = BOId;
            ViewData["PropertyName"] = PropertyName;
            ViewData["Original"] = Original;
            return View();

        }

        [HttpPost]
        [Route("SaveDynamicTranslation")]
        public ActionResult SaveDynamicTranslation()
        {
            var BOName = Request.Form["BOName"];
            var BOId = Convert.ToInt64(Request.Form["BOId"]);
            var PropertyName = Request.Form["PropertyName"];
            var Languages = new Language().Populate();
            foreach (var lang in Languages.Values)
            {
                var Language = (Language)lang;
                if (!string.IsNullOrEmpty(Request.Form["Language_" + lang.Id.ToString()]))
                {
                    if (!string.IsNullOrEmpty(Request.Form["TranslationId_" + lang.Id.ToString()]) && long.TryParse(Request.Form["TranslationId_" + lang.Id.ToString()], out var translationId))
                    {
                        var Translation = new Translation(translationId);
                        Translation.BOTable = BOName;
                        Translation.BOId = BOId;
                        Translation.BOField = PropertyName;
                        Translation.Language = Language;
                        Translation.Value = Request.Form["Language_" + lang.Id.ToString()];
                        if (Translation.Id > 0)
                        {
                            Translation.Update(Translation);
                        }
                        else
                        {
                            Translation.Insert(Translation);
                        }

                        var Translations = Translation.Populate();
                    }
                }
            }
            return Json(new RequestResult() { Result = RequestResultType.Success });

        }
    }
}