using Lib.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools.Utils
{
    public class DateTimeZone
    {
        public static DateTime Now
        {
            get
            {
                if (!string.IsNullOrEmpty(Config.GetConfigValue("UTC")) && double.TryParse(Config.GetConfigValue("UTC"), out double utc))
                    return DateTime.UtcNow.AddHours(utc);
                else return DateTime.Now;
            }
        }
    }
}
