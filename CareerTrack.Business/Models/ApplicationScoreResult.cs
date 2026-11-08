using System;
using System.Collections.Generic;
using System.Text;

namespace CareerTrack.Business.Models
{
    public class ApplicationScoreResult
    {
        public string Status { get; set; } = string.Empty;
        public int DaysSinceApplied { get; set; }
        public decimal Salary { get; set; }
        public int StatusScore { get; set; }
        public int DaysScore { get; set; }
        public int SalaryScore { get; set; }
        public int TotalScore { get; set; }
    }
}