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
    public class EditProjectModel : PageModel
    {
        [BindProperty]
        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                using (WebDBManager webm = new WebDBManager(Comm.SERVER_URL))
                {
                    Project = await webm.ReadProjectAsync(id.Value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return NotFound();
            }

            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                using (WebDBManager webm = new WebDBManager(Comm.SERVER_URL))
                {
                    bool result = await webm.Update(Project, "Project", Project.Id);
                    if (!result)
                        return StatusCode(400, Project);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(400, ex);
            }

            return RedirectToPage("./Index");
        }
    }
}
