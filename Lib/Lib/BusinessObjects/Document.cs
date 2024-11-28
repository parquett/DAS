// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Document type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Lib.Tools.BO;
    using Lib.AdvancedProperties;
    using Lib.Tools.Utils;

    /// <summary>
    /// The document.
    /// </summary>
    [Serializable]
    [Bo(
        LogRevisions = false
      )
    ]
    public class Document : ItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        public Document()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Document(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        [Db(_Ignore = true)]
        private string _RelativePath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Ext { get; set; }

        [Db(_Ignore = true)]
        public string File
        {
            get
            {
                if (!string.IsNullOrEmpty(_RelativePath))
                    return Lib.Tools.Utils.URLHelper.GetUrl(_RelativePath);

                if (!string.IsNullOrEmpty(FileName))
                    return Lib.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/" + FileName + "." + Ext);

                if (!string.IsNullOrEmpty(Name))
                    return Lib.Tools.Utils.URLHelper.GetUrl(Config.GetConfigValue("UploadURL") + "/" + Name + "." + Ext);

                return "";
            }
        }

        public string GetPhysicalPath()
        {
            if (!string.IsNullOrEmpty(_RelativePath))
                return _RelativePath;

            var rootDirectory = Config.GetConfigValue("UploadFullPart");
            if (!string.IsNullOrEmpty(rootDirectory))
            {
                rootDirectory = rootDirectory.TrimEnd('\\');

                if (!string.IsNullOrEmpty(FileName))
                    return $@"{rootDirectory}\{FileName}.{Ext}";
            }
            else
            {
                return Config.GetConfigValue("UploadPart") + "/" + FileName + "." + Ext;
            }
            return "";
        }
    }
}