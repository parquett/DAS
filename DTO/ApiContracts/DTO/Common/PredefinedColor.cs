using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class ColorDTO

    {
        public string Code { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
