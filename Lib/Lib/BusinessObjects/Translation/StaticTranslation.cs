using lib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaticTranslation.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Translations class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects.Translations
{
    using Lib.AdvancedProperties;
    using Lib.BusinessObjects;
    using Lib.Cache;
    using Lib.Tools.BO;
    using Lib.Tools.Security;
    using Lib.Tools.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Linq;

    /// <summary>
    /// The static translations.
    /// </summary>

    public class StaticTranslation : ItemBase
    {

        public StaticTranslation()
            : base(0)
        {
        }

        public StaticTranslation(long id)
            : base(id)
        {
        }

        #region Properties

        [Template(Mode = Template.Name)]
        public string Key { get; set; }

        [Template(Mode = Template.SearchDropDown)]
        public StaticTranslationAssembly StaticTranslationAssembly { get; set; }

        [Template(Mode = Template.LinkItems), LinkItem(LinkType = typeof(StaticTranslationValue)), Access(DisplayMode = DisplayMode.Simple)]
        public Dictionary<long, ItemBase> Values { get; set; }

        #endregion

        #region Populate

        public static List<StaticTranslation> Populate()
        {
            var conn = DataBase.ConnectionFromContext();

            var selectCommand = new SqlCommand("StaticTranslation_Populate", conn) { CommandType = CommandType.StoredProcedure };

            var Items = new List<StaticTranslation>();

            var ds = new DataSet();

            var da = new SqlDataAdapter();
            da.SelectCommand = selectCommand;
            da.Fill(ds);

            ds.Tables[0].TableName = "Keys";
            ds.Tables[1].TableName = "Values";

            ds.Relations.Add(
            ds.Tables["Keys"].Columns["StaticTranslationId"],
            ds.Tables["Values"].Columns["StaticTranslationId"]);

            ds.Relations[0].Nested = true;
            ds.Relations[0].RelationName = "Keys_Values";

            foreach (DataRow dr in ds.Tables["Keys"].Rows)
            {
                var sTranslation = new StaticTranslation();
                sTranslation.Id = dr["StaticTranslationId"] != DBNull.Value ? (long)dr["StaticTranslationId"] : 0;
                sTranslation.Key = dr["Key"] != DBNull.Value ? (string)dr["Key"] : "";
                sTranslation.StaticTranslationAssembly = new StaticTranslationAssembly();
                sTranslation.StaticTranslationAssembly.Id = dr["StaticTranslationAssemblyId"] != DBNull.Value ? (long)dr["StaticTranslationAssemblyId"] : 0;
                sTranslation.StaticTranslationAssembly.Key = dr["StaticTranslationAssemblyKey"] != DBNull.Value ? (string)dr["StaticTranslationAssemblyKey"] : "";
                sTranslation.Values = new Dictionary<long, ItemBase>();
                var datarowsValues = dr.GetChildRows(ds.Relations["Keys_Values"]);
                foreach (DataRow drValue in datarowsValues)
                {
                    var Value = new StaticTranslationValue();
                    Value.Id = drValue["StaticTranslationValueId"] != DBNull.Value ? (long)drValue["StaticTranslationValueId"] : 0;
                    Value.Value = drValue["Value"] != DBNull.Value ? (string)drValue["Value"] : "";
                    Value.Language = new Language();
                    Value.Language.Id = drValue["LanguageId"] != DBNull.Value ? (long)drValue["LanguageId"] : 0;
                    sTranslation.Values.Add(Value.Id, Value);
                }
                if (!Items.Contains(sTranslation))
                    Items.Add(sTranslation);
            }

            return Items;
        }
        public static StaticTranslation PopulateOne(StaticTranslation Translation)
        {
            var conn = DataBase.ConnectionFromContext();

            var selectCommand = new SqlCommand("StaticTranslation_PopulateOne", conn) { CommandType = CommandType.StoredProcedure };

            selectCommand.Parameters.Add(new SqlParameter("@TranslationId", SqlDbType.BigInt) { Value = Translation.Id });

            var sTranslation = new StaticTranslation();

            var ds = new DataSet();

            var da = new SqlDataAdapter();
            da.SelectCommand = selectCommand;
            da.Fill(ds);

            ds.Tables[0].TableName = "Keys";
            ds.Tables[1].TableName = "Values";

            ds.Relations.Add(
            ds.Tables["Keys"].Columns["StaticTranslationId"],
            ds.Tables["Values"].Columns["StaticTranslationId"]);

            ds.Relations[0].Nested = true;
            ds.Relations[0].RelationName = "Keys_Values";

            foreach (DataRow dr in ds.Tables["Keys"].Rows)
            {
                sTranslation = (StaticTranslation)new StaticTranslation().FromDataRow(dr);
                sTranslation.Values = new Dictionary<long, ItemBase>();
                var datarowsValues = dr.GetChildRows(ds.Relations["Keys_Values"]);
                foreach (DataRow drValue in datarowsValues)
                {
                    var Value = (StaticTranslationValue)new StaticTranslationValue().FromDataRow(drValue);
                    sTranslation.Values.Add(Value.Id, Value);
                }
            }

            return sTranslation;
        }

        public static Dictionary<long, ItemBase> PopulateWithPaging(List<SortParameter> SortParameters,
                                                       int iPagingStart,
                                                       int iPagingLen,
                                                       StaticTranslationAssembly Assembly,
                                                       string GlobalSearch,
                                                       string Key,
                                                       out long itotal)
        {
            var conn = DataBase.ConnectionFromContext();

            string CacheKey = "";

            var ip = HttpContextHelper.Current.Connection.RemoteIpAddress;

            var ParamKeys = "";
            var Translations = new Dictionary<long, ItemBase>();

            if (iPagingLen > 0)
            {
                ParamKeys += "_ps_" + iPagingStart.ToString();
                ParamKeys += "_pl_" + iPagingLen.ToString();
            }

            itotal = 0;

            //CacheKey = CacheKeyProcessor.BuildKey(new List<string>() { CacheGroup.Request, ParamKeys });
            //var itotalKey = CacheKeyProcessor.BuildKey(new List<string>() { CacheGroup.Request, ParamKeys, "itotal" });
            if (true /*!(CacheProcessor.Exists(CacheKey) && CacheProcessor.Exists(itotalKey) && CacheProcessor.Exists(idisplaytotalKey))*/)
            {
                #region Build Query
                var cmd = new SqlCommand("StaticTranslation_PopulateWithPaging", conn) { CommandType = CommandType.StoredProcedure };

                if (iPagingLen > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@PagingStart", SqlDbType.Int) { Value = iPagingStart });
                    cmd.Parameters.Add(new SqlParameter("@PagingLen", SqlDbType.Int) { Value = iPagingLen });
                }
                if (!string.IsNullOrEmpty(GlobalSearch))
                    cmd.Parameters.Add(new SqlParameter("@GlobalSearch", SqlDbType.NVarChar, 200) { Value = GlobalSearch.Replace(" ", "").Replace("<br/>", "").ToUpper() });
                if (!string.IsNullOrEmpty(Key))
                    cmd.Parameters.Add(new SqlParameter("@Key", SqlDbType.NVarChar, 200) { Value = Key });
                if (Assembly != null && Assembly.Id > 0)
                {
                    cmd.Parameters.Add(new SqlParameter("@AssemblyId", SqlDbType.BigInt) { Value = Assembly.Id });
                }

                var dbparam = new SqlParameter("@TotalDisplayValues", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                cmd.Parameters.Add(dbparam);


                var ds = new DataSet();

                var da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(ds);

                ds.Tables[0].TableName = "Keys";
                ds.Tables[1].TableName = "Values";

                ds.Relations.Add(
                ds.Tables["Keys"].Columns["StaticTranslationId"],
                ds.Tables["Values"].Columns["StaticTranslationId"]);

                ds.Relations[0].Nested = true;
                ds.Relations[0].RelationName = "Keys_Values";

                foreach (DataRow dr in ds.Tables["Keys"].Rows)
                {
                    var Item = (StaticTranslation)new StaticTranslation().FromDataRow(dr);
                    Item.Values = new Dictionary<long, ItemBase>();
                    var datarowsValues = dr.GetChildRows(ds.Relations["Keys_Values"]);
                    foreach (DataRow drValue in datarowsValues)
                    {
                        var Value = (StaticTranslationValue)new StaticTranslationValue().FromDataRow(drValue);
                        Item.Values.Add(Value.Id, Value);
                    }
                    Translations.Add(Item.Id, Item);
                }

                itotal = Convert.ToInt64(cmd.Parameters["@TotalDisplayValues"].Value);
            }
            #endregion
            return Translations;
        }

        #endregion

        public static bool TranslationExist(StaticTranslation item)
        {
            var conn = DataBase.ConnectionFromContext();
            var selectCommand = new SqlCommand("StaticTranslation_PopulateByField", conn) { CommandType = CommandType.StoredProcedure };
            selectCommand.Parameters.Add(new SqlParameter("@Key", SqlDbType.NVarChar, 100) { Value = item.Key });
            selectCommand.Parameters.Add(new SqlParameter("@AssemblyId", SqlDbType.BigInt) { Value = item.StaticTranslationAssembly.Id });
            StaticTranslation trans = null;
            using (var rdr = selectCommand.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    trans = (StaticTranslation)new StaticTranslation().FromDataRow(rdr);
                }

                rdr.Close();
            }
            if (trans != null && trans.Id > 0)
                return true;
            return false;
        }

        public void ClearValues()
        {
            var conn = DataBase.ConnectionFromContext();
            var cmd = new SqlCommand("StaticTranslation_ClearValues", conn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.Add(new SqlParameter("@TranslationId", SqlDbType.BigInt) { Value = Id });
            cmd.ExecuteNonQuery();
        }

        public static List<ItemBase> LoadColumns()
        {
            var ColumList = new List<ItemBase>();
            var i = 1;
            var Languages = new Language().Populate();
            foreach (var language in Languages.Values)
            {
                var oReport = new ItemBase(i++);
                oReport.SetName(language.GetName());
                ColumList.Add(oReport);
            }

            return ColumList;
        }

        public override bool Delete(Dictionary<long, ItemBase> dictionary, out string Reason, string Comment = "", SqlConnection connection = null, User user = null)
        {
            var conn = DataBase.ConnectionFromContext();
            var Ids = string.Join(",", dictionary.Keys.Select(x => x.ToString()));
            var selectCommand = new SqlCommand("StaticTranslation_Delete", conn) { CommandType = CommandType.StoredProcedure };
            selectCommand.Parameters.Add(new SqlParameter("@Ids", SqlDbType.BigInt) { Value = Ids });
            selectCommand.ExecuteNonQuery();
            Reason = "";
            return true;
        }
    }



}
