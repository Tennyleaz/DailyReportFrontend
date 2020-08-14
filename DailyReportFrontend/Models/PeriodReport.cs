using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DailyReportFrontend.Models
{
    public class PeriodReport
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        public int ReoprtID { get; set; }

        [Required]
        public int ProjectID { get; set; }

        [Display(Name = "Project Name")]
        [StringLength(60, MinimumLength = 2)]
        [Required]
        public string ProjectName { get; set; }

        public string Message { get; set; }

        public string Version { get; set; }
    }
}
