using Lib.Tools.AdminArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRMLib.Tools.AdminArea
{
    public class AdminAreaGroup : Lib.Tools.AdminArea.AdminAreaGroup
    {
        public AdminAreaGroupenum Group { get; set; }
        public AdminAreaGroup Parent { get; set; }

        public static AdminAreaGroup None = new AdminAreaGroup() { Group = AdminAreaGroupenum.None, Name = "None" };
        public static AdminAreaGroup Settings = new AdminAreaGroup() { Group = AdminAreaGroupenum.Settings, Name = "Settings", Icon = "cogs" };
        public static AdminAreaGroup UserManagement = new AdminAreaGroup() { Group = AdminAreaGroupenum.UserManagement, Name = "User Management", Icon = "users" };
        public static AdminAreaGroup Alerts = new AdminAreaGroup() { Group = AdminAreaGroupenum.Alerts, Name = "Alerts", Icon = "file-text-o" };
        public static AdminAreaGroup Incidents = new AdminAreaGroup() { Group = AdminAreaGroupenum.Incidents, Name = "Incidents", Icon = "pencil-square-o" };
        public static AdminAreaGroup Reports = new AdminAreaGroup() { Group = AdminAreaGroupenum.Reports, Name = "Reports", Icon = "print" };
        public static AdminAreaGroup IPAdress = new AdminAreaGroup() { Group = AdminAreaGroupenum.IPAdress, Name = "IPAdress", Icon = "desktop" };
        public static AdminAreaGroup Documents = new AdminAreaGroup() { Group = AdminAreaGroupenum.Documents, Name = "Documents", Icon = "book" };
        public static AdminAreaGroup Monitoring = new AdminAreaGroup() { Group = AdminAreaGroupenum.Monitoring, Name = "Monitoring", Icon = "cogs" };
        public static AdminAreaGroup System = new AdminAreaGroup() { Group = AdminAreaGroupenum.System, Name = "System" };

        public static Dictionary<AdminAreaGroupenum, AdminAreaGroup> Groups
        {
            get
            {
                Dictionary<AdminAreaGroupenum, AdminAreaGroup> list = new Dictionary<AdminAreaGroupenum, AdminAreaGroup>();

                list.Add(None.Group, None);
                list.Add(UserManagement.Group, UserManagement);
                list.Add(Settings.Group, Settings);
                list.Add(Alerts.Group, Alerts);
                list.Add(Incidents.Group, Incidents);
                list.Add(Reports.Group, Reports);
                list.Add(IPAdress.Group, IPAdress);
                list.Add(Documents.Group, Documents);
                list.Add(System.Group, System);
                list.Add(Monitoring.Group, Monitoring);

                return list;
            }
        }
    }
}
