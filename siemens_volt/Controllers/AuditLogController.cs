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
    [Authorize]
    public class AuditLogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuditLogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Generate
        public IActionResult Generate()
        {
            return View();
        }

        // POST: Logger/Generate
        [HttpPost]
        public async Task<string> Generate([FromBody] AuditLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                AuditLog logger = new AuditLog(DateTime.Now, model.Action, model.Description);
                _context.Add(logger);
                await _context.SaveChangesAsync();

                return model.Action;
            }
            return "ERROR_INVALID_STATE";
        }

        // GET: AuditLog
        public async Task<IActionResult> IndexAsync(string sortOrder)
        {
            ViewBag.TimestampSortParm = sortOrder == "Timestamp" ? "timestamp_desc" : "Timestamp";
            ViewBag.ActionSortParm = sortOrder == "Action" ? "action_desc" : "Action";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "description_desc" : "Description";

            var auditLogs = await _context.AuditLog.ToListAsync();
            var orderedAuditLogs = auditLogs.OrderByDescending(a => a.Timestamp);

            switch (sortOrder)
            {
                case "Timestamp":
                    orderedAuditLogs = orderedAuditLogs.OrderBy(s => s.Timestamp);
                    break;                
                case "Action":
                    orderedAuditLogs = orderedAuditLogs.OrderBy(s => s.Action);
                    break;
                case "action_desc":
                    orderedAuditLogs = orderedAuditLogs.OrderByDescending(s => s.Action);
                    break;
                case "Description":
                    orderedAuditLogs = orderedAuditLogs.OrderBy(s => s.Description);
                    break;
                case "description_desc":
                    orderedAuditLogs = orderedAuditLogs.OrderByDescending(s => s.Description);
                    break;
            }
            return View(orderedAuditLogs);
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

        // GET: AuditLog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuditLog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Timestamp,Action,Description")] AuditLog auditLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auditLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(auditLog);
        }

        // GET: AuditLog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditLog = await _context.AuditLog.FindAsync(id);
            if (auditLog == null)
            {
                return NotFound();
            }
            return View(auditLog);
        }

        // POST: AuditLog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Timestamp,Action,Description")] AuditLog auditLog)
        {
            if (id != auditLog.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auditLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuditLogExists(auditLog.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(IndexAsync));
            }
            return View(auditLog);
        }

        // GET: AuditLog/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: AuditLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auditLog = await _context.AuditLog.FindAsync(id);
            _context.AuditLog.Remove(auditLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(IndexAsync));
        }

        private bool AuditLogExists(int id)
        {
            return _context.AuditLog.Any(e => e.ID == id);
        }
    }
}
