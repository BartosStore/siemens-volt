using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using siemens_volt.Models;

namespace siemens_volt.Data
{
    /**
     * Identity je sluzba pro jednoduchou implementaci autentikace.
     * Dokaze generovat schema.
     */
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /**
         * Timto je zajisteno vytvareni AuditLog tabulky.
         */
        public DbSet<AuditLog> AuditLog { get; set; }
    }
}
