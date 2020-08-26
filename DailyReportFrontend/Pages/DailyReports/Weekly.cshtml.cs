using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DailyReportFrontend.Data;
using DailyReportFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DailyReportFrontend.Pages.DailyReports
{
    public class WeeklyModel : PageModel
    {
        private bool mergeSameProject = true;

        public List<AccumulatedReport> WeeklyReport { get; set; }

        public async Task OnGetAsync()
        {
            // calculate date
            DayOfWeek dayOfWeek = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(DateTime.Today);
            int shiftedDay = (int)dayOfWeek;
            DateTime startDay = DateTime.Today.AddDays(-shiftedDay);
            DateTime endDay = startDay.AddDays(7);

            IEnumerable<PeriodReport> alist;
            using (WebDBManager wdManager = new WebDBManager(Comm.SERVER_URL))
            {
                alist = await wdManager.ReadPeriodReportAsync(startDay, endDay);
            }

            WeeklyReport = new List<AccumulatedReport>();

            string pattern = @"^修正 *(M|m)antis *上的問題(：|:)";
            Regex regex = new Regex(pattern);
            foreach (PeriodReport drm in alist)
            {
                AccumulatedReport ar = WeeklyReport.FirstOrDefault(x => x.ProjectId == drm.ProjectID);
                if (ar == null)
                {
                    ar = new AccumulatedReport() { ProjectName = drm.ProjectName, ProjectId = drm.ProjectID, ProjectVersion = drm.Version };
                    WeeklyReport.Add(ar);
                }

                // check if line contains mantis number
                var match = regex.Match(drm.Message);
                if (match.Success)
                {
                    // add mantis number to project
                    string mantisLine = drm.Message.Substring(match.Length, drm.Message.Length - match.Length);
                    string[] mantisItems = mantisLine.Split('、');
                    foreach (string item in mantisItems)
                        ar.MantisList.Add(item);
                }
                else
                {
                    // add message line to corresponding project
                    ar.ProjectContent.Add(drm.Message);
                }

            }

            // sort and merge
            if (mergeSameProject)
                WeeklyReport = MergeSameReports(WeeklyReport);
            WeeklyReport.Sort();
        }

        private List<AccumulatedReport> MergeSameReports(IReadOnlyList<AccumulatedReport> reports)
        {
            Dictionary<string, AccumulatedReport> dict = new Dictionary<string, AccumulatedReport>();
            foreach (var report in reports)
            {
                if (report.ProjectName == null)
                    report.ProjectName = string.Empty;

                if (dict.ContainsKey(report.ProjectName))
                {
                    dict[report.ProjectName].ProjectContent.AddRange(report.ProjectContent);
                    dict[report.ProjectName].MantisList.UnionWith(report.MantisList);
                    // TODO: use highest version number
                }
                else
                    dict.Add(report.ProjectName, report);
            }

            return dict.Values.ToList();
        }
    }
}
