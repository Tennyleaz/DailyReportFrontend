using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyReportFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DailyReportFrontend.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DailyReportFrontend.Pages.DailyReports
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IList<PeriodReport> Periods { get; set; }
        public SelectList Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedProject { get; set; }
        public string NameSort { get; set; }
        public string DateSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder)
        {
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            DateSort = sortOrder == "Date" ? "date_desc" : "Date";

            IEnumerable<PeriodReport> periods;
            using (WebDBManager webm = new WebDBManager(Comm.SERVER_URL))
            {
                try
                {
                    periods = await webm.ReadAllPeriodReportAsync();
                    // 初始化分類
                    HashSet<string> projectNames = new HashSet<string>();
                    var projects = await webm.ReadAllProjectsAsync();
                    foreach (var p in projects)
                    {
                        projectNames.Add(p.ProjectName);
                    }
                    // 這些沒用
                    projectNames.Remove(string.Empty);
                    projectNames.Remove("WorlCard 8.6.2");
                    Genres = new SelectList(projectNames.ToList().OrderBy(x => x));
                }
                catch (Exception ex)
                {
                    Console.Write(ex);
                    periods = new List<PeriodReport>();
                }
            }

            // 分類
            if (!string.IsNullOrEmpty(SelectedProject))
            {
                periods = periods.Where(x => x.ProjectName == SelectedProject);
            }

            switch (sortOrder)
            {
                case "Date":
                    Periods = periods.OrderBy(s => s.Date).ToList();
                    break;
                case "date_desc":
                    Periods = periods.OrderByDescending(s => s.Date).ToList();
                    break;
                case "name_desc":
                    Periods = periods.OrderByDescending(s => s.ProjectName).ToList();
                    break;
                default:
                    // 預設是ProjectName排序
                    Periods = periods.ToList();
                    break;
            }
        }
    }
}
