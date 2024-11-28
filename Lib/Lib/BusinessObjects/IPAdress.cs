// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPAdress.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Sex type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects
{
    using System;

    using Lib.AdvancedProperties;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using ApiContracts.Enums;
    using System.Collections.Generic;

    [Bo(Group = AdminAreaGroupenum.IPAdress
      , ModulesAccess = (long)(Modulesenum.ControlPanel)
      , DisplayName = "IPAdress"
      , SingleName = "IPAdress"
      , LogRevisions = true)]
    public class IPAdress : ItemBase
    {
        #region Constructors

        public IPAdress()
            : base(0)
        {
        }
          
        public IPAdress(long id)
            : base(id)
        {
        }
        #endregion

        #region Properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        #endregion

    }
}