using lib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Translation.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the StaticTranslations type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects.Translations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;

    using Lib.AdvancedProperties;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using Lib.Tools.AdminArea;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;

    /// <summary>
    /// The static translations.
    /// </summary>
    [Serializable]
    public class Translation : ItemBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        public Translation()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Translation"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Translation(long id)
            : base(id)
        {
        }

        #region properties

        /// <summary>
        /// Gets or sets the alias.
        /// </summary>
        /*[Common(Order = 0, EditTemplate = EditTemplates.SimpleInput, DisplayName = "Admin_ID",
            ControlClass = CssClass.Wide),
         Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced)]*/

        [Template(Mode = Template.Name)]
        public string BOField { get; set; }

        [Template(Mode = Template.Name)]
        public string BOTable { get; set; }

        [Template(Mode = Template.Number)]
        public long BOId { get; set; }

        [Template(Mode = Template.ParentDropDown)]
        public Language Language { get; set; }

        [Template(Mode = Template.Description), Access(DisplayMode = DisplayMode.Advanced | DisplayMode.Simple), Db(ParamSize = -1)]
        public string Value { get; set; }


        #endregion

        public static List<Translation> Populate()
        {
            var conn = DataBase.ConnectionFromContext();

            var selectCommand = new SqlCommand("Translation_Populate", conn) { CommandType = CommandType.StoredProcedure };

            var Items = new List<Translation>();

            using (var rdr = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var Item = new Translation();
                    Item.Id = rdr["TranslationId"] != null ? (long)rdr["TranslationId"] : 0;
                    Item.BOField = rdr["BOField"] != null ? (string)rdr["BOField"] : "";
                    Item.BOTable = rdr["BOTable"] != null ? (string)rdr["BOTable"] : "";
                    Item.BOId = rdr["BOId"] != null ? (long)rdr["BOId"] : 0;
                    Item.Value = rdr["Value"] != null ? (string)rdr["Value"] : "";
                    Item.Language = new Language();
                    Item.Language.Id = rdr["LanguageId"] != null ? (long)rdr["LanguageId"] : 0;
                    Item.Language.ShortName = rdr["LanguageShortName"] != null ? (string)rdr["LanguageShortName"] : "";
                    if (!Items.Contains(Item))
                    {
                        //Debug.Write("Found: " + Item.Value);
                        Items.Add(Item);
                    }
                }
                rdr.Close();
            }
            return Items;
        }

        public static Dictionary<long, ItemBase> PupulateByKey(string BOName, long BOId, string PropertyName)
        {
            var conn = DataBase.ConnectionFromContext();
            var Translations = new Dictionary<long, ItemBase>();
            var cmd = new SqlCommand("Translation_PopulateByKey", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@BOField", SqlDbType.NVarChar, 50) { Value = PropertyName });
            cmd.Parameters.Add(new SqlParameter("@BOTable", SqlDbType.NVarChar, 50) { Value = BOName });
            cmd.Parameters.Add(new SqlParameter("@BOId", SqlDbType.BigInt) { Value = BOId });

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var Item = new Translation().FromDataRow(rdr);
                    Translations.Add(Item.Id, Item);
                }
                rdr.Close();
            }
            return Translations;
        }

        static List<Translation> Translations = null;

        public static string Load(string BOField, string BOTable, long BOId, Language pLanguage, string defaultvalue = "")
        {
            string translated = "";

            Language currentLanguage = Language.Current();
            if (currentLanguage == null)
            {
                currentLanguage = Language.Romanian;
                currentLanguage.ShortName = "ro";
            }

            if (Translations == null)
            {
                Translations = Populate();
            }

            if (Translations != null
                && Translations.Any(x => x.BOField == BOField
                                      && x.BOTable == BOTable
                                      && x.BOId == BOId
                                      && x.Language == pLanguage))
            {
                translated = Translations.First(x => x.BOField == BOField
                                                  && x.BOTable == BOTable
                                                  && x.BOId == BOId
                                                  && x.Language == pLanguage).Value;
            }

            return !string.IsNullOrEmpty(translated) ? translated : defaultvalue;
        }

        public static string SanitizeHTML(string html)
        {
            var doc = new HtmlAgilityPack.HtmlDocument
            {
                OptionFixNestedTags = true,
                OptionCheckSyntax = true,
                OptionAutoCloseOnEnd = true
            };
            doc.LoadHtml(html);

            //string tdText = doc.DocumentNode.SelectSingleNode(".//td[@class='team_a_col home']")?.InnerText.Trim();

            return doc.DocumentNode.InnerText.Trim();
        }


    }
}