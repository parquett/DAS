using SecurityCRM.ApiContracts.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class MonitoringDataDTO
    {
        public string? name { get; set; }
        public List<decimal>? data { get; set; }
        public string? color { get; set; }

    }
}
