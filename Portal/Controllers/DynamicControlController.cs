using Lib.Tools.BO;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Weblib.Helpers;
using Lib.BusinessObjects;
using SecurityCRMLib.BusinessObjects;
using System.Globalization;

using Lib.AdvancedProperties;
using System.ComponentModel;
using Lib.Tools.Revisions;
using Lib.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SecurityCRM.Controllers
{
    public class DynamicControlController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public DynamicControlController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        [HttpPost]
        public ActionResult Load()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];

            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));

            BoAttribute boproperties = null;
            if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
            {
                boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
            }

            var items = item.PopulateFrontEndItems();

            var viewData = item.LoadFrontEndViewdata();
            if (viewData != null && viewData.Count > 0)
            {
                foreach (var key in viewData.Keys)
                {
                    ViewData[key] = viewData[key];
                }
            }
            return View(item.GetType().Name, items);
        }

        [HttpPost]
        public ActionResult LoadNewItem(string ParentId)
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }

            var Namespace = Request.Form["Namespace"];
            ViewData["ParentId"] = Convert.ToInt64(ParentId);

            var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));

            return View(item.GetType().Name+"AddRow");
        }

        [HttpPost]
        public ActionResult Save()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var Namespace = Request.Form["Namespace"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));
                
                BoAttribute boproperties = null;
                if (item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true).Length > 0)
                {
                    boproperties = (Lib.AdvancedProperties.BoAttribute)item.GetType().GetCustomAttributes(typeof(Lib.AdvancedProperties.BoAttribute), true)[0];
                }


                item.CollectFromForm();
                item.SaveForm();

                var items = item.PopulateFrontEndItems();
                
                var viewData = item.LoadFrontEndViewdata();
                if (viewData != null && viewData.Count > 0)
                {
                    foreach (var key in viewData.Keys)
                    {
                        ViewData[key] = viewData[key];
                    }
                }

                ViewData["NewItemId"] = item.Id;

                return View(item.GetType().Name, items);

            }
            catch (Exception ex)
            {
                //throw ex;
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }

        [HttpPost]
        public ActionResult Delete()
        {
            if (!Authentication.CheckUser(this.HttpContext))
            {
                return this.Json(new RequestResult() { RedirectURL = Config.GetConfigValue("LoginPage") + "?ReturnUrl=" + System.Net.WebUtility.UrlEncode("Account/Manage"), Result = RequestResultType.Reload });
            }
            try
            {
                var Namespace = Request.Form["Namespace"];

                var item = (ItemBase)Activator.CreateInstance(Type.GetType(Namespace + ", " + Namespace.FirstOrDefault().Split('.')[0], true));
                item.Id = Convert.ToInt64(Request.Form["Id"]);

                Revision.Insert(new Revision() { BOId = item.Id, BOName = "", Comment = "Cancel", Date = DateTimeZone.Now, Table = item.GetType().Name, Type = OperationTypes.Cancel });

                var itemsToDelete = new Dictionary<long, ItemBase>();
                itemsToDelete.Add(item.Id, item);
                item.Delete(itemsToDelete);

                if (Request.Form["ClientReload"] == "0")
                {
                    var items = item.PopulateFrontEndItems();

                    var viewData = item.LoadFrontEndViewdata();
                    if (viewData != null && viewData.Count > 0)
                    {
                        foreach (var key in viewData.Keys)
                        {
                            ViewData[key] = viewData[key];
                        }
                    }

                    return View(item.GetType().Name, items);
                }

                return this.Json(new RequestResult() { Result = RequestResultType.Success });
            }
            catch (Exception ex)
            {
                //throw ex;
                return this.Json(new RequestResult() { Result = RequestResultType.Fail, Message = ex.ToString() });
            }
        }
    }
}
