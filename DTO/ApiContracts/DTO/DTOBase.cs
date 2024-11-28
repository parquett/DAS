using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class DTOBase
    {
        public DTOBase() { }

        public long Id { get; set; }
        public DateTime? DateCreated { get; set; } = DateTime.MinValue;
        public string? CreatedBy { get; set; } = string.Empty;
    }
}
