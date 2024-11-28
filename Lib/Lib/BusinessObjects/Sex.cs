// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Sex.cs" company="SecurityCRM">
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

    /// <summary>
    /// The sex.
    /// </summary>
    public class Sex : ItemBase
    {
        #region Static Sex
        
        /// <summary>
        /// The male.
        /// </summary>
        public static readonly Sex Male = new Sex(1, "Male");

        /// <summary>
        /// The female.
        /// </summary>
        public static readonly Sex Female = new Sex(2, "Female");

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        public Sex()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Sex(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sex"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public Sex(int id, string name)
            : base(id)
        {
            this.Name = name;
        }

        #endregion

        #region Sex Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.TranslatableName)]
        [Validation(ValidationType = ValidationTypes.Required),
         Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced,
             EditableFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin), Lib.AdvancedProperties.Translate(Translatable = true)]
        public string Name { get; set; }
        
        #endregion        
        
    }
}