using SecurityCRM.ApiContracts.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class LineChartDTO
    {
        public List<string> options { get; set; }
        public List<MonitoringDataDTO> data { get; set; }

    }
}
