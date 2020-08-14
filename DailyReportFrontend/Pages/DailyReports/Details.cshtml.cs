using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyReportFrontend.Data;
using DailyReportFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DailyReportFrontend.Pages.DailyReports
{
    public class DetailsModel : PageModel
    {
        public PeriodReport Period { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pid, int? rid)
        {
            if (pid == null || rid == null)
            {
                return NotFound();
            }

            using (WebDBManager webm = new WebDBManager(Comm.SERVER_URL))
            {
                Project project;
                DailyReport dailyReport;
                try
                {
                    project = await webm.ReadProjectAsync(pid.Value);
                    dailyReport = await webm.ReadDailyReportAsync(rid.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return NotFound();
                }

                if (project == null || dailyReport == null)
                    return NotFound();

                Period = new PeriodReport();
                Period.ProjectName = project.ProjectName;
                Period.ProjectID = project.Id;
                Period.Version = project.Version;
                Period.ReoprtID = dailyReport.Id;
                Period.Date = dailyReport.Date;
                Period.Message = dailyReport.Message;
            }

            return Page();
        }
    }
}
