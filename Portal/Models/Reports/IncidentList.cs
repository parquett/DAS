// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncidentList.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the IncidentList type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using ApiContracts.Enums;
using SecurityCRMLib.BusinessObjects;
using Weblib.Models.Common;
using System;
using Lib.Tools.BO;
using Lib.AdvancedProperties;
using System.ComponentModel;
using Lib.Tools.Security;
using Lib.Tools.Utils;
using Lib.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using Lib.Tools.Revisions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Extensions;
using System.Reflection;
using Lib.Helpers;
using lib;
using Lib.Tools.AdminArea;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using SecurityCRM.Controllers;
using Lib.ErrorHandler;
using Lib.Tools.Controls;
namespace SecurityCRM.Models.Reports
{
    /// <summary>
    /// The IncidentList controller.
    /// </summary>
    [Bo(DisplayName = "IncidentList")]
    public class IncidentList : ReportBase
    {
        public IncidentList() : base()
        {
        }

        public override string GetLink()
        {
            return "ControlPanel/Edit/Incident/Lib.BusinessObjects" + IncidentId.ToString();
        }

        [Common(DisplayName = "ID", _Sortable = true), Template(Mode = Template.Name), Db(Sort = DbSortMode.Desc)]
        public long IncidentId { get; set; }
        [Common(_Searchable = true, DisplayName = "Incident"), Access(DisplayMode = Lib.AdvancedProperties.DisplayMode.Simple | Lib.AdvancedProperties.DisplayMode.Search)]
        public string Name { get; set; }
        [Template(Mode = Template.DateRange), Common(_Searchable = true, DisplayName = "DateCreated"), Access(DisplayMode = Lib.AdvancedProperties.DisplayMode.Simple | Lib.AdvancedProperties.DisplayMode.Search)]
        public DateRange DateCreated { get; set; }
        [Template(Mode = Template.Name), Common(_Searchable = true, DisplayName = "CreatedBy"), Access(DisplayMode = Lib.AdvancedProperties.DisplayMode.Simple | Lib.AdvancedProperties.DisplayMode.Search)]
        public string CreatedBy { get; set; }
    }
}
