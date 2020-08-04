using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siemens_volt.Models
{
    /**
     * Tato trida slouzi jako interface dat posilanych z JS do kontrolleru.
     */
    public class AuditLogViewModel
    {
        public string Action { get; set; }
        public string Description { get; set; }
    }
}
