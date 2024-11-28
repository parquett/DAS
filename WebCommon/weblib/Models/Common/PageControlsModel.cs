// ------------------------------------public --------------------------------------------------------------------------------
// <copyright file="TextboxModel.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the TextboxModel type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Models.Common
{
    using Lib.Models.Common;
    using System.Collections.Generic;
    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The textbox model.
    /// </summary>
    public class PageControlsModel : iBaseControlModel
    {
        public PageControlsModel()
        {
            Delete = true;
            Save = true;
            DisableDynamicControlsOnSave = true;
            SaveCaption = "Save";
            DeleteCaption = "Cancel";
        }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Object { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public bool Delete { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string DeleteCaption { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public bool Save { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string SaveCaption { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public bool DisableDynamicControlsOnSave { get; set; }

        public List<ButtonModel> Buttons { get; set; }
    }
}
