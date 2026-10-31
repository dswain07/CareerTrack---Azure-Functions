namespace CareerTrack.Client.Models
{
    public class InterviewModel
    {
        public int Id { get; set; }
        public string InterviewType { get; set; } = string.Empty;
        public DateTime InterviewDate { get; set; }
        public string? Location { get; set; }
        public string? Outcome { get; set; }
        public string? Notes { get; set; }
        public int JobApplicationId { get; set; }
    }
}
