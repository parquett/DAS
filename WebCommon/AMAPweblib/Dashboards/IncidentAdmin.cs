using Lib.BusinessObjects;
using Lib.Tools.Utils;
using SecurityCRMweblib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using weblib.Dashboards;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;

namespace SecurityCRMweblib.Dashboards
{
    public class IncidentAdmin : BaseDashboard
    {
        public DashboardEnum DashboardType = DashboardEnum.Incident;
        public override void LoadIncident(ViewDataDictionary ViewData, Incident IncidentCount)
        {
            ViewData["List"] = IncidentCount.LoadLatestsIncidents();
            ViewData["GroupedList"] = IncidentCount.LoadUsersPerIncidents();
            ViewData["Dashboard"] = DashboardType;
        }
    }
}
