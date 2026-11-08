using System;
using System.Collections.Generic;
using System.Text;

namespace CareerTrack.Business.Models
{
    public class FollowUpRecommendationResult
    {
        public string Status { get; set; } = string.Empty;
        public int DaysSinceApplied { get; set; }
        public bool ShouldFollowUp { get; set; }
        public string Recommendation { get; set; } = string.Empty;
    }
}