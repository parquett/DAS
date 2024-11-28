// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Language.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Language type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects.Translations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Lib.AdvancedProperties;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using Lib.Tools.AdminArea;
    using lib;
    using Newtonsoft.Json;
    using Microsoft.AspNetCore.Http;
    using Lib.Helpers;
    using ApiContracts.Enums;
    using Lib.BusinessObjects;

    public class Language : ItemBase
    {

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        public Language()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Language"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Language(long id)
            : base(id)
        {
        }
        #endregion

        #region Fields
        public static Language English = new Language(1) { ShortName = "En" };
        public static Language Romanian = new Language(2) { ShortName = "Ro" };

        #endregion

        public static Language Current()
        {
            SessionHelper<Language>.Pull(SessionItems.Language, out var CurrentLanguage);
            return CurrentLanguage;
        }

        public override string GetCaption()
        {
            return "FullName";
        }

        /// <summary>
        /// Gets or sets the short name.
        /// </summary>        
        [Common(Order = 0), Template(Mode = Template.Name)]
        public string ShortName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Common(Order = 1), Template(Mode = Template.Name)]//TranslatableName
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        [Common(Order = 1, _Searchable = true, DisplayName = "Culture"), Template(Mode = Template.Name)]//TranslatableName
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        [Common(Order = 2, DisplayName = "Image"), Template(Mode = Template.Image)]
        public Graphic Image { get; set; }

        /// <summary>
        /// Gets or sets the enable.
        /// </summary>
        [Common(Order = 3), Template(Mode = Template.CheckBox)]
        public bool Enabled { get; set; }

    }

}
