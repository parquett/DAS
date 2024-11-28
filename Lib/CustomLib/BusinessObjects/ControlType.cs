// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlType.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The ControlType class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using System.Collections.Generic;
    using ApiContracts.Enums;


    public class ControlType : ItemBase
    {
        #region Constructors

        public ControlType()
            : base(0)
        {
        }

        public ControlType(long id)
            : base(id)
        {
        }
        #endregion

        public static ControlType Bool = new ControlType(4);
        public static ControlType DropDown = new ControlType(3);
        public static ControlType TextArea = new ControlType(2);
        public static ControlType Input = new ControlType(1);
        public static ControlType InputText = new ControlType(5);
        public static ControlType InputMethod = new ControlType(6);
        public static ControlType InputWithIncertitude = new ControlType(7);

        public static Dictionary<long, Lib.Tools.BO.ItemBase> BoolVals
        {
            get
            {
                var vals = new Dictionary<long, Lib.Tools.BO.ItemBase>();
                vals.Add(1, new ControlType(1) { Name = "Negative" });
                vals.Add(2, new ControlType(2) { Name = "Positive" });
                vals.Add(3, new ControlType(3) { Name = "Suspected" });
                return vals;
            }
        }
        #region Properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Template(Mode = Template.CheckBox)]
        public bool Enabled { get; set; }

        #endregion
    }
}