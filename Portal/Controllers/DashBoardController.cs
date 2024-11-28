using Lib.BusinessObjects;
using Lib.Helpers;
using Lib.Tools.BO;
using Lib.Tools.Security;
using Lib.Tools.Utils;

using SecurityCRMLib.BusinessObjects;
using SecurityCRMweblib.Dashboards;
using SecurityCRMweblib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using weblib.Dashboards;
using Weblib.Controllers;
using Microsoft.AspNetCore.Mvc;
using SecurityCRM;
using Person = SecurityCRMLib.BusinessObjects.Person;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace SecurityCRM.Controllers
{
    public class DashBoardController : SecurityCRMweblib.Controllers.FrontEndController
    {
        public DashBoardController(ICompositeViewEngine viewEngine) : base(viewEngine)
        {
        }

        //
        // GET: /DashBoard/
        private IDashboard Dashboard
        {
            get
            {
                var currentUser = Authentication.GetCurrentUser();
                var currentPerson = Person.Current();

                if (currentUser.HasAtLeastOnePermission((long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin))
                {
                    return new UserAdmin();
                }
                else if (currentUser.HasAtLeastOnePermission((long)SecurityCRM.ApiContracts.Enums.Permissions.VIncident))
                {
                    return new IncidentAdmin();
                }
                return null;
            }
        }

        public ActionResult Index(Incident incident = null)
        {
            var currentUser = Authentication.GetCurrentUser();

            ViewData["DashboardListCount"] = 0;

            if (currentUser.HasAtLeastOnePermission((long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin))
            {
                if (Dashboard != null)
                {
                    Dashboard.Load(ViewData, currentUser);
                }
            }
            else if (currentUser.HasAtLeastOnePermission((long)SecurityCRM.ApiContracts.Enums.Permissions.VIncident))
            {
                if (Dashboard != null)
                {
                    Dashboard.LoadIncident(ViewData, incident);
                }
            }
            return View();
        }

        //public ActionResult Refresh(long lastId, int count)
        //{
        //    var currentUser = Authentication.GetCurrentUser();

        //    var Data = new Dictionary<string, object>();

        //    if (Dashboard != null)
        //        Data= Dashboard.RefreshWidget(ViewData, ControllerContext, TempData, currentUser, lastId, count,Request.Form["widgetitems"]);

        //    return this.Json(new RequestResult() { Result = RequestResultType.Success, Data = Data });
        //}

    }
}
