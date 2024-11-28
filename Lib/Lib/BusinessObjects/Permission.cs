// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Permission.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Permission type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.BusinessObjects
{
    using Lib.Tools.AdminArea;
    using Lib.AdvancedProperties;
    using Lib.Tools.BO;
    using System.Collections.Generic;
    using ApiContracts.Enums;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// The permissions.
    /// </summary>
    [Bo(Group = AdminAreaGroupenum.UserManagement
      , ModulesAccess = (long)Modulesenum.SMI
      , DisplayName = "Permissions"
      , SingleName = "Permission"
      , EditAccess = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin
      , CreateAccess = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin
      , DeleteAccess = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin
      , ReadAccess = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin
      , LogRevisions = false
      , RevisionsAccess = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin
      , Icon = "laptop")]
    public class Permission : ItemBase
    {
        #region Static Permission

        /// <summary>
        /// The none.
        /// </summary>
        public static readonly Permission None = new Permission(0, 0);

        /// <summary>
        /// The administrator area access.
        /// </summary>
        public static readonly Permission CPAccess = new Permission(1, 1) { Name = "Control Panel Access" };

        /// <summary>
        /// The super admin.
        /// </summary>
        public static readonly Permission SMIAccess = new Permission(2, 2) { Name = "Super Administrator" };

        public static readonly Permission SuperAdmin = new Permission(3, 4) { Name = "Sistem Management Interface Access" };

        public static readonly Permission API = new Permission(4, 8) { Name = "API" };

        public static readonly Permission VIncident = new Permission(5, 16) { Name = "View Dashboard Incidents" };

        public static readonly Permission VIP = new Permission(6, 32) { Name = "View IP Dashboard" };

        public static readonly Permission VAdmins = new Permission(7, 64) { Name = "View Admins Dashboard" };

        public static readonly Permission CIncindent = new Permission(8, 128) { Name = "Pie Chart Incidents" };
        //public static readonly Permission AddRequests = new Permission(5, 16) { Name = "Register Requests" };

        //public static readonly Permission AddResults = new Permission(6, 32) { Name = "Perform Investigations" };

        //public static readonly Permission ConfirmResults = new Permission(7, 64) { Name = "Confirm Investigations" };

        //public static readonly Permission AccessRequests = new Permission(8, 128) { Name = "Access to Requests" };

        //public static readonly Permission CanChangeLaboratory = new Permission(9, 256) { Name = "Can switch Laboratory" };

        //public static readonly Permission CanAccessAllSections = new Permission(10, 512) { Name = "Access to All Sections" };

        //public static readonly Permission AccessInvestigations = new Permission(11, 1024) { Name = "Access to Investigations" };

        //public static readonly Permission AccessAccountingAdvanced = new Permission(12, 2048) { Name = "Access to Bookkeeping Advanced" };

        //public static readonly Permission AccessConsumablesDistribution = new Permission(13, 4096) { Name = "Access to Consumables Distribution" };

        //public static readonly Permission AccessInventory = new Permission(14, 8192) { Name = "Access to Advanced Deposit" };

        //public static readonly Permission EditConsumale = new Permission(15, 16384) { Name = "Access to Consumables for Investigation" };

        //public static readonly Permission ConsumaleResponsable = new Permission(16, 32768) { Name = "Responsible for supplies in the Section" };

        //public static readonly Permission AccessDocumentManagement = new Permission(17, 65536) { Name = "Access to Documents" };

        //public static readonly Permission AccessAgency = new Permission(18, 131072) { Name = "Access Centralizator" };

        ////public static readonly Permission AccessProtocol = new Permission(26, 262144) { Name = "Access to Protocol" };


        //public static readonly Permission CanChangeCustomsPost = new Permission(20, 524288) { Name = "Can change Customs Post" };

        //public static readonly Permission ImportRequests = new Permission(21, 1048576) { Name = "Import Samples from Extern Sistem" };

        //public static readonly Permission CancelProtocols = new Permission(22, 2097152) { Name = "Cancel Protocol" };

        //public static readonly Permission AccessDocumentAdd = new Permission(23, 4194304) { Name = "Can Add Documents" };

        //public static readonly Permission MilkAnalisys = new Permission(24, 8388608) { Name = "Milk Analisys" };

        //public static readonly Permission MilkManagement = new Permission(25, 16777216) { Name = "Milk Management" };

        //public static readonly Permission Patient = new Permission(26, 262144) { Name = "Patient" };
        #endregion

        public override string GetName()
        {
            return Name;
        }
        public static Dictionary<long, ItemBase> LoadPermissions(long Permissions)
        {
            var Perms = new Dictionary<long, ItemBase>();

            if (Tools.Security.Permissions.HasPermissions(Permissions, CPAccess.Value))
                Perms.Add(CPAccess.Id, CPAccess);
            if (Tools.Security.Permissions.HasPermissions(Permissions, SuperAdmin.Value))
                Perms.Add(SuperAdmin.Id, SuperAdmin);
            if (Tools.Security.Permissions.HasPermissions(Permissions, API.Value))
                Perms.Add(API.Id, API);
            if (Tools.Security.Permissions.HasPermissions(Permissions, SMIAccess.Value))
                Perms.Add(SMIAccess.Id, SMIAccess);
            if (Tools.Security.Permissions.HasPermissions(Permissions, VIncident.Value))
                Perms.Add(VIncident.Id, VIncident);
            if (Tools.Security.Permissions.HasPermissions(Permissions, VIP.Value))
                Perms.Add(VIP.Id, VIP);
            if (Tools.Security.Permissions.HasPermissions(Permissions, VAdmins.Value))
                Perms.Add(VAdmins.Id, VAdmins);
            if (Tools.Security.Permissions.HasPermissions(Permissions, CIncindent.Value))
                Perms.Add(CIncindent.Id, CIncindent);
            return Perms;
        }
        public static Permission LoadPermission(long id)
        {
            if (CPAccess.Id == id)
                return CPAccess;
            if (SuperAdmin.Id == id)
                return SuperAdmin;
            if (SMIAccess.Id == id)
                return SMIAccess;
            if (API.Id == id)
                return API;
            if (VIncident.Id == id)
                return VIncident;
            if (VIP.Id == id)
                return VIP;
            if (VAdmins.Id == id)
                return VAdmins;
            if (CIncindent.Id == id)
                return CIncindent;
            return None;
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        public Permission()
            : base(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public Permission(long id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Permission"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public Permission(long id, long value)
            : base(id)
        {
            this.Value = value;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Common(Order = 0), Template(Mode = Template.Name)]
        [Validation(ValidationType = ValidationTypes.Required),
    Access(DisplayMode = DisplayMode.Search | DisplayMode.Simple | DisplayMode.Advanced,
        EditableFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin), Lib.AdvancedProperties.Translate(Translatable = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        [Common(Order = 1), Template(Mode = Template.Number),
        Access(EditableFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin, VisibleFor = (long)SecurityCRM.ApiContracts.Enums.Permissions.SuperAdmin)]
        public long Value { get; set; }
     
        #endregion
    }
}
