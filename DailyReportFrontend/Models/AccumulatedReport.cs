using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyReportFrontend.Models
{
    public class AccumulatedReport : IComparable<AccumulatedReport>
    {
        public int ProjectId;
        public string ProjectName;
        public string ProjectVersion;
        public List<string> ProjectContent;
        public HashSet<string> MantisList;

        public AccumulatedReport()
        {
            ProjectId = 0;
            ProjectName = string.Empty;
            ProjectVersion = string.Empty;
            ProjectContent = new List<string>();
            MantisList = new HashSet<string>();
        }

        public string DisplayString => this.ToString();

        public new string ToString()
        {
            string message = ProjectName + " " + ProjectVersion + ":\n";
            int i = 0;
            for (i = 0; i < ProjectContent.Count; i++)
            {
                ProjectContent[i] = ProjectContent[i].TrimStart(' ');
                message += (i + 1).ToString() + ". " + ProjectContent[i] + Environment.NewLine;
            }
            if (MantisList.Count > 0)
                message += (i + 1).ToString() + ". 修正 Mantis 問題：" + string.Join("、", MantisList) + "。" + Environment.NewLine;
            return message;
        }

        int IComparable<AccumulatedReport>.CompareTo(AccumulatedReport other)
        {
            if (other == null)
                return -1;
            else
            {
                int rtn = ProjectName.CompareTo(other.ProjectName);
                if (rtn == 0)
                {
                    if (string.IsNullOrEmpty(ProjectVersion) && string.IsNullOrEmpty(other.ProjectVersion))
                        return 0;
                    if (string.IsNullOrEmpty(ProjectVersion))
                        return 1;
                    return ProjectVersion.CompareTo(other.ProjectVersion);
                }
                else
                    return rtn;
            }
        }
    }
}
