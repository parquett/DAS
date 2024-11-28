using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO.PushNotifications
{
    public class PushNotificationRequest
    {
        public string to { get; set; }
        public NotificationMessageBody notification { get; set; }
        public object? data { get; set; }
    }
    public class NotificationMessageBody
    {
        public string title { get; set; }
        public string body { get; set; }
        public string? sound { get; set; }
    }
    public class NotificationConfig
    {
        public string ServerKey { get; set; }
    }

}
