using siemens_volt.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace siemens_volt.Models
{
    public class AuditLogger
    {
        private readonly ApplicationDbContext _context;

        public AuditLogger(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task WriteNewRecord(AuditLog auditLog)
        {
            this._context.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}
