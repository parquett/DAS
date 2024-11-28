// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Country.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The Country class.
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

    public class Country : ItemBase
    {
        #region Constructors

        public Country()
            : base(0)
        {
        }

        public Country(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Template(Mode = Template.Name), Lib.AdvancedProperties.Translate(Translatable = true)]
        public string Name { get; set; }

        [Template(Mode = Template.Name)]
        public string Code { get; set; }

        [Template(Mode = Template.String),Access(DisplayMode =Lib.AdvancedProperties.DisplayMode.Advanced| Lib.AdvancedProperties.DisplayMode.Simple)]
        public string SpecialCode { get; set; }

        [Template(Mode = Template.CheckBox)]
        public bool EU { get; set; }

        #endregion
    }
}