// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticTranslationAssembly.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Translations class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects.Translations
{
    using Lib.AdvancedProperties;
    using Lib.Tools.AdminArea;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using ApiContracts.Enums;

    /// <summary>
    /// The static translations.
    /// </summary>

    public class StaticTranslationAssembly : ItemBase
    {

        public StaticTranslationAssembly()
            : base(0)
        {
        }

        public StaticTranslationAssembly(long id)
            : base(id)
        {
        }

        #region Fields
        public static StaticTranslationAssembly AdminArea = new StaticTranslationAssembly(1);
        public static StaticTranslationAssembly FrontEnd = new StaticTranslationAssembly(2);
        #endregion

        #region Properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Template(Mode = Template.String)]
        public string Key { get; set; }

        #endregion

    }
}