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
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Models.Common;
    using System;
    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The textbox model.
    /// </summary>
    public class PhotoModel : iBaseControlModel
    {
        public PhotoModel()
        {
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public Graphic Value { get; set; }
        
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets if textbox is required.
        /// </summary>
        public ValidationTypes ValidationType { get; set; }

        /// <summary>
        /// Gets or sets if textbox is ReadOnly.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets if textbox is Disabled.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets textbox required message.
        /// </summary>
        public string RequiredMessage { get; set; }

        /// <summary>
        /// Gets or sets the Width.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the Height.
        /// </summary>
        public int Height { get; set; }

    }
}
