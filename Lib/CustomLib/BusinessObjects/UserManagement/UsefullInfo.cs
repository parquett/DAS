// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UsefullInfo.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   The Useful Info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SecurityCRMLib.BusinessObjects
{
    using System;
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using ApiContracts.Enums;


    [Serializable]
    [Bo(Group = AdminAreaGroupenum.Settings
      , DisplayName = "Information"
      , SingleName = "Info")]
    public class UsefullInfo : ItemBase
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsefullInfo"/> class.
        /// </summary>
        public UsefullInfo()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UsefullInfo"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public UsefullInfo(long id)
            : base(id)
        {
        }
        #endregion

        public override string GetName()
        {
            return Id.ToString();
        }

        public override void SetName(object name)
        {
            Text = name!=null?name.ToString():"";
        }

        #region Properties

        [Common(Order = 0), Template(Mode = Template.Html)]
        public string Text { get; set; }
        /*
        /// <summary>
        /// Gets the Image.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        [Common(Order = 1, EditTemplate = EditTemplates.ImageUpload)]
        [Access(DisplayMode = DisplayMode.Simple)]
        public Graphic Image { get; set; }
        */
        #endregion
    }
}