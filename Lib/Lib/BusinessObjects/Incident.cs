namespace Lib.BusinessObjects
{
    using System;
    using System.Timers;

    using Lib.AdvancedProperties;
    using Lib.Tools.BO;
    using Lib.Tools.AdminArea;
    using ApiContracts.Enums;
    using System.Collections.Generic;
    using Lib.Tools.Utils;
    using System.Data.SqlClient;
    using System.Data;
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.AspNetCore.Mvc;

    [Bo(Group = AdminAreaGroupenum.Incidents
      , ModulesAccess = (long)(Modulesenum.VIncident)
      , DisplayName = "Incidents"
      , SingleName = "Incidents"
      , LogRevisions = true)]
    public class Incident : AggregateBase
    {
        #region Constructors

        public Incident()
            : base(0)
        {
            this.Id = 0;
            InitializeTimer();
        }

        public Incident(long id)
            : base(id)
        {
            this.Id = id;
            InitializeTimer();
        }

        #endregion

        #region Properties

        [Template(Mode = Template.Name)]
        public string Name { get; set; }

        [Common(EditTemplate = EditTemplates.Hidden), Db(_Editable = false, _Populate = false, _Ignore = true)]
        public int IncidentCount { get; set; }

        [Common(EditTemplate = EditTemplates.Hidden), Db(_Editable = false, _Populate = false, _Ignore = true)]
        public string CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        #endregion

        public override int GetCount() => IncidentCount;

        public Dictionary<long, Incident> LoadLatestsIncidents(long lastestId = 0)
        {
            var conn = DataBase.ConnectionFromContext();

            Dictionary<long, Incident> Incidents = new Dictionary<long, Incident>();

            var cmd = new SqlCommand("Incident_Populate_Latests", conn) { CommandType = CommandType.StoredProcedure };

            if (lastestId > 0)
                cmd.Parameters.Add(new SqlParameter("@LastestId", SqlDbType.BigInt) { Value = lastestId });

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var incident = (Incident)(new Incident()).FromDataRow(rdr);
                    Incidents.Add(incident.Id, incident);
                }

                rdr.Close();
            }

            return Incidents;
        }

        public override string GetChartName() => GetName();
        public override string GetName() => Name;

        public Dictionary<long, AggregateBase> LoadUsersPerIncidents()
        {
            var conn = DataBase.ConnectionFromContext();
            var incidents = new Dictionary<long, AggregateBase>();

            var cmd = new SqlCommand("Users_Populate_Incident", conn) { CommandType = CommandType.StoredProcedure };

            using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleResult))
            {
                while (rdr.Read())
                {
                    var incident = (Incident)(new Incident()).FromDataRow(rdr);
                    int IncidentCount = rdr.GetInt32(rdr.GetOrdinal("IncidentCount"));
                    incident.IncidentCount = IncidentCount;
                    incidents.Add(incident.Id, incident);
                }
                rdr.Close();
            }

            return incidents;
        }

        private Timer _timer;

        private void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Interval = 3000; 
            _timer.AutoReset = true;
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                TransferDataBetweenDatabases();
            }
            catch (Exception ex)
            {
            }
        }
        public void TransferDataBetweenDatabases()
        {
            var sourceConnString = "Data Source=.\\MSSQLSERVER01; Initial Catalog=HelpDesk; pwd=SED465g; user id=sa; MultipleActiveResultSets=True;TrustServerCertificate=True;";
            var destConnString = "Data Source=.\\MSSQLSERVER01; Initial Catalog=SecurityCRM; pwd=SED465g; user id=sa; MultipleActiveResultSets=True;TrustServerCertificate=True;";

            using (var sourceConn = new SqlConnection(sourceConnString))
            using (var destConn = new SqlConnection(destConnString))
            {
                sourceConn.Open();
                destConn.Open();

                var existingIncidentIds = new HashSet<long>();
                var selectExistingIdsQuery = "SELECT IncidentId FROM SecurityCRM.dbo.Incident;";
                using (var cmdExistingIds = new SqlCommand(selectExistingIdsQuery, destConn))
                using (var reader = cmdExistingIds.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        existingIncidentIds.Add(Convert.ToInt64(reader["IncidentId"]));
                    }
                }

                using (var cmdIdentityOn = new SqlCommand("SET IDENTITY_INSERT SecurityCRM.dbo.Incident ON", destConn))
                {
                    cmdIdentityOn.ExecuteNonQuery();
                }

                var selectQuery = $@"
        SELECT IncidentId, Name, DeletedBy, CreatedBy, DateCreated, DateUpdated 
        FROM HelpDesk.dbo.Incident
        WHERE IncidentId NOT IN ({string.Join(",", existingIncidentIds)});
        ";

                using (var cmdSelect = new SqlCommand(selectQuery, sourceConn))
                using (var reader = cmdSelect.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long IncidentId = Convert.ToInt64(reader["IncidentId"]);
                        var name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : null;
                        long? deletedBy = reader["DeletedBy"] != DBNull.Value ? Convert.ToInt64(reader["DeletedBy"]) : (long?)null;
                        long createdBy = Convert.ToInt64(reader["CreatedBy"]);
                        DateTime? dateCreated = reader["DateCreated"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["DateCreated"]) : null;
                        DateTime? dateUpdated = reader["DateUpdated"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["DateUpdated"]) : null;

                        InsertIncident(destConn, IncidentId, name, deletedBy, createdBy, dateCreated, dateUpdated);
                    }
                }

                using (var cmdIdentityOff = new SqlCommand("SET IDENTITY_INSERT SecurityCRM.dbo.Incident OFF", destConn))
                {
                    cmdIdentityOff.ExecuteNonQuery();
                }
            }
        }




        private void InsertIncident(SqlConnection destConn, long IncidentId, string name, long? deletedBy, long createdBy, DateTime? dateCreated, DateTime? dateUpdated)
        {
            var insertQuery = @"
    INSERT INTO SecurityCRM.dbo.Incident (IncidentId, Name, DeletedBy, CreatedBy, DateCreated, DateUpdated)
    VALUES (@IncidentId, @Name, @DeletedBy, @CreatedBy, @DateCreated, @DateUpdated);
    ";

            using (var cmdInsert = new SqlCommand(insertQuery, destConn))
            {
                cmdInsert.Parameters.AddWithValue("@IncidentId", IncidentId);
                cmdInsert.Parameters.AddWithValue("@Name", name);
                cmdInsert.Parameters.AddWithValue("@DeletedBy", deletedBy ?? (object)DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@CreatedBy", createdBy);
                cmdInsert.Parameters.AddWithValue("@DateCreated", dateCreated ?? (object)DBNull.Value);
                cmdInsert.Parameters.AddWithValue("@DateUpdated", dateUpdated ?? (object)DBNull.Value);

                cmdInsert.ExecuteNonQuery();
            }
        }

    }
}
