// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayMode.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The DisplayMode class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using Lib.Tools.AdminArea;
    using ApiContracts.Enums;

    public class DisplayMode : ItemBase
    {
        public static DisplayMode Simple = new DisplayMode(1);
        public static DisplayMode Advanced = new DisplayMode(2);
        public static DisplayMode Search = new DisplayMode(3);
        public static DisplayMode AdvancedEdit = new DisplayMode(4);
        public static DisplayMode Print = new DisplayMode(5);
        public static DisplayMode PrintSearch = new DisplayMode(6);
        public static DisplayMode CSV = new DisplayMode(7);
        public static DisplayMode Excell = new DisplayMode(8);

        #region Constructors

        public DisplayMode()
            : base(0)
        {
        }

        public DisplayMode(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Common(Order = 1), Template(Mode = Template.Number),
        Access(EditableFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin, VisibleFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin)]
        public long Value { get; set; }

        #endregion

        public static Dictionary<long, ItemBase> FromDataTable(DataRow[] dt, DataSet ds)
        {
            var DisplayModes = new Dictionary<long, ItemBase>();
            foreach (var dr in dt)
            {
                var obj = (new DisplayMode()).FromDataRow(dr);
                if (!DisplayModes.ContainsKey(obj.Id))
                {
                    DisplayModes.Add(obj.Id, obj);
                }
            }

            return DisplayModes;
        }
    }
}