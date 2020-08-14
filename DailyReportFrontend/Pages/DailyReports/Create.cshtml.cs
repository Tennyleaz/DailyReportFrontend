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
    public class CreateModel : PageModel
    {
        public IActionResult OnGet()
        {
            // The Page method creates a PageResult object that renders the Create.cshtml page.
            return Page();
        }

        [BindProperty]
        public Project NewProject { get; set; }

        [BindProperty]
        public DailyReport NewDailyReport { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // The Page method creates a PageResult object that renders the Create.cshtml page.
                return Page();
            }

            using (WebDBManager webm = new WebDBManager(Comm.SERVER_URL))
            {
                int projectId = await webm.TryAddProjectAsync(NewProject);
                NewDailyReport.ProjectId = projectId;
                NewDailyReport = await webm.AddNew<DailyReport>(NewDailyReport, nameof(DailyReport));
            }

            return RedirectToPage("./Index");
        }
    }
}
