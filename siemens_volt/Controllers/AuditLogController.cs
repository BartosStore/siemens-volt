﻿using System;
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
