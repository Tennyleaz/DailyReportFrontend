using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DailyReportFrontend.Models
{
    public class DailyReport
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
