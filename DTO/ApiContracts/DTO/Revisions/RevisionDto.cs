using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityCRM.ApiContracts.DTO
{
    public class RevisionDTO : DTOBase
    {
        public string Table { get; set; } = string.Empty;
        public long BOId { get; set; }
        public string BOName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Json { get; set; } = string.Empty;
    }
}
