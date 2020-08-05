using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using siemens_volt.Data;
using siemens_volt.Models;

namespace siemens_volt
{
    /**
     * Kontroller obsluhujici view pod AuditLog/.
     * Diky Authorize je dostupny pouze pro prihlasene uzivatele.
     * 
     */
    [Authorize]
    public class AuditLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        /**
         * Jednoduchy pristup k page pomoci GET pozadavku.
         */
        // GET: Generate
        public IActionResult Generate()
        {
            return View();
        }

        /**
         * Metoda prijimajici POST pozadavky nesouci data k zapisu do DB.
         */
        // POST: Logger/Generate
        [HttpPost]
        public async Task<string> Generate([FromBody] AuditLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Zde je slozen objekt AuditLog a zapsan do DB.
                // Diky _context muzeme pres ORM jednoduse zapisovat do DB.
                // Pro znovupouzitelnost byla vytvorena trida AuditLogger.
                AuditLog auditLog = new AuditLog(DateTime.Now, model.Action, model.Description);
                await new AuditLogger(_context).WriteNewRecord(auditLog);

                return model.Action;
            }
            return "ERROR_INVALID_STATE";
        }

        /**
         * Defaultni page pro /AuditLog.
         * Vraci data z DB a pritom nabizi sortovani, vyhledavani a strankovani.
         */
        // GET: AuditLog
        public async Task<IActionResult> Index(
            string sortOrder,
            string currentFilter,
            string searchString,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["TimestampSortParm"] = sortOrder == "Timestamp" ? "timestamp_desc" : "Timestamp";
            ViewData["ActionSortParm"] = sortOrder == "Action" ? "action_desc" : "Action";
            ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "description_desc" : "Description";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var auditLogs = from a in _context.AuditLog
                            select a;

            // search
            if (!String.IsNullOrEmpty(searchString))
            {
                auditLogs = auditLogs.Where(a => a.Action.ToLower().Contains(searchString.ToLower())
                    || a.Description.ToLower().Contains(searchString.ToLower())
                );
            }

            // sort
            switch (sortOrder)
            {
                case "Timestamp":
                    auditLogs = auditLogs.OrderBy(s => s.Timestamp);
                    break;
                case "Action":
                    auditLogs = auditLogs.OrderBy(s => s.Action);
                    break;
                case "action_desc":
                    auditLogs = auditLogs.OrderByDescending(s => s.Action);
                    break;
                case "Description":
                    auditLogs = auditLogs.OrderBy(s => s.Description);
                    break;
                case "description_desc":
                    auditLogs = auditLogs.OrderByDescending(s => s.Description);
                    break;
            }

            int pageSize = 10;
            // pro ucely strankovani byla vytvorena PaginatedList
            return View(await PaginatedList<AuditLog>.CreateAsync(auditLogs.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: AuditLog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _context.AuditLog
                .FirstOrDefaultAsync(m => m.ID == id);
            if (auditLog == null)
            {
                return NotFound();
            }

            return View(auditLog);
        }

        private bool AuditLogExists(int id)
        {
            return _context.AuditLog.Any(e => e.ID == id);
        }
    }
}
