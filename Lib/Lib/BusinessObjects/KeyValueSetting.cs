// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Contacts.cs" company="Quanex">
//   Copyright ©  2018
// </copyright>
// <summary>
//   The Contact.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects
{
    using System;

    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using System.Data;
    using ApiContracts.Enums;
    using Lib.AdvancedProperties;
    using Lib.Tools.AdminArea;
    using Lib.Tools.Utils;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The Contact.
    /// </summary>
    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , ModulesAccess = (long)(Modulesenum.SMI)
       , DisplayName = "Global Settings"
       , SingleName = "Global Setting"
       , LogRevisions = true)]
    public class KeyValueSetting : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        public KeyValueSetting()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Contact"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public KeyValueSetting(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetName()
        {
            return Key;
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Name)]
        public string Key { get; set; }

        [Common(Order = 1), Template(Mode = Template.Name)]
        public string Value { get; set; }

        #endregion
    }
}