using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace siemens_volt.Models
{
    /**
     * Model class definuje podobu DB tabulky.
     * Podle zadani slouzi k ulozeni dat generovanych z JS.
     */
    public class AuditLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy hh:mm}")]
        public DateTime Timestamp { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }

        public AuditLog(DateTime timestamp, string action, string description)
        {
            Timestamp = timestamp;
            Action = action;
            Description = description;
        }
    }
}