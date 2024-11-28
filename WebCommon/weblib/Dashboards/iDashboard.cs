using Lib.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace weblib.Dashboards
{
    public interface IDashboard
    {
        void Load(ViewDataDictionary ViewData, User currentUser);

        Dictionary<string, object> RefreshWidget(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, User currentUser, long lastId, int count, string widgetitems);

        void LoadIncident(ViewDataDictionary ViewData, Incident IncidentUser);

        Dictionary<string, object> RefreshWidgetIncident(ViewDataDictionary ViewData, ControllerContext ControllerContext, TempDataDictionary TempData, Incident IncidentUser, long lastId, int count, string widgetitems);
    }
}
