using SecurityCRM.ApiContracts.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class CPULoadChartDTO
    {
        public string type { get; set; }
        public string name { get; set; }
        public decimal value { get; set; }
        public string color { get; set; }

    }
}
