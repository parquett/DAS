// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticTranslationValue.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Translations class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects.Translations
{
    using Lib.AdvancedProperties;
    using Lib.Cache;
    using Lib.Tools.BO;
    using Lib.Tools.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;

    /// <summary>
    /// The static translations.
    /// </summary>

    public class StaticTranslationValue : ItemBase
    {

        public StaticTranslationValue()
            : base(0)
        {
        }

        public StaticTranslationValue(long id)
            : base(id)
        {
        }

        #region Properties

        [Template(Mode = Template.Parent)]
        public StaticTranslation StaticTranslation { get; set; }

        [Template(Mode = Template.Name)]
        public string Value { get; set; }

        [Template(Mode = Template.ParentDropDown)]
        public Language Language { get; set; }

        #endregion

        public override string GetName()
        {
            return Value;
        }

        #region Populate

        //public static Dictionary<string, StaticTranslationValue> Populate()
        //{
        //    var conn = DataBase.ConnectionFromContext();

        //    var selectCommand = new SqlCommand("StaticTranslationValue_Populate", conn) { CommandType = CommandType.StoredProcedure };

        //    var Items = new Dictionary<string, StaticTranslationValue>();

        //    using (var rdr = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
        //    {
        //        while (rdr.Read())
        //        {
        //            //var Item = (StaticTranslationValue)(new StaticTranslationValue()).FromDataRow(rdr);
        //            var Item = new StaticTranslationValue();
        //            Item.Id = rdr["StaticTranslationValueId"] != null ? (long)rdr["StaticTranslationValueId"] : 0;
        //            Item.Key = rdr["Key"] != null ? (string)rdr["Key"] : "";
        //            Item.Original = rdr["Original"] != null ? (string)rdr["Original"] : "";
        //            Item.Translation = rdr["Translation"] != null ? (string)rdr["Translation"] : "";
        //            Item.Language = new Language();
        //            Item.Language.Id = rdr["LanguageId"] != null ? (long)rdr["LanguageId"] : 0;
        //            Item.Language.ShortName = rdr["LanguageShortName"] != null ? (string)rdr["LanguageShortName"] : "";
        //            var Key = Item.Key + "_" + Item.StaticTranslationValueAssembly.Name.ToUpper() + "_" + Item.Language.ShortName.ToUpper();
        //            if (!Items.ContainsKey(Key))
        //            {
        //                Debug.Write("Found: " + Key);
        //                Items.Add(Key, Item);
        //            }
        //        }
        //        rdr.Close();
        //    }
        //    return Items;
        //}

        //public static Dictionary<long, ItemBase> PopulateWithPaging(List<SortParameter> SortParameters,
        //                                               int iPagingStart,
        //                                               int iPagingLen,
        //                                               StaticTranslationValueAssembly Assembly,
        //                                               string GlobalSearch,
        //                                               out long itotal)
        //{
        //    var conn = DataBase.ConnectionFromContext();

        //    string CacheKey = "";

        //    var ip = HttpContextHelper.Current.Request.UserHostAddress;

        //    var ParamKeys = "";
        //    var Translations = new Dictionary<long, ItemBase>();

        //    if (iPagingLen > 0)
        //    {
        //        ParamKeys += "_ps_" + iPagingStart.ToString();
        //        ParamKeys += "_pl_" + iPagingLen.ToString();
        //    }

        //    itotal = 0;

        //    CacheKey = CacheKeyProcessor.BuildKey(new List<string>() { CacheGroup.ImportData, ParamKeys });
        //    var itotalKey = CacheKeyProcessor.BuildKey(new List<string>() { CacheGroup.ImportData, ParamKeys, "itotal" });
        //    if (true/* || !(CacheProcessor.Exists(CacheKey) && CacheProcessor.Exists(itotalKey) && CacheProcessor.Exists(idisplaytotalKey))*/)
        //    {
        //        #region Build Query
        //        var cmd = new SqlCommand("StaticTranslationValue_PopulateWithPaging", conn) { CommandType = CommandType.StoredProcedure };

        //        if (iPagingLen > 0)
        //        {
        //            cmd.Parameters.Add(new SqlParameter("@PagingStart", SqlDbType.Int) { Value = iPagingStart });
        //            cmd.Parameters.Add(new SqlParameter("@PagingLen", SqlDbType.Int) { Value = iPagingLen });
        //        }
        //        if (!string.IsNullOrEmpty(GlobalSearch))
        //            cmd.Parameters.Add(new SqlParameter("@GlobalSerach", SqlDbType.NVarChar, 200) { Value = GlobalSearch });
        //        if (Assembly != null && Assembly.Id > 0)
        //        {
        //            cmd.Parameters.Add(new SqlParameter("@AssemblyId", SqlDbType.BigInt) { Value = Assembly.Id });
        //        }

        //        var dbparam = new SqlParameter("@TotalDisplayValues", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
        //        cmd.Parameters.Add(dbparam);

        //        using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
        //        {
        //            while (rdr.Read())
        //            {
        //                var Translation = (StaticTranslationValue)(new StaticTranslationValue()).FromDataRow(rdr);
        //                Translations.Add(Translation.Id, Translation);
        //            }
        //            rdr.Close();
        //        }
        //        itotal = Convert.ToInt64(cmd.Parameters["@TotalDisplayValues"].Value);
        //    }
        //    #endregion
        //    return Translations;
        //}


        #endregion

    }
}