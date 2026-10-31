namespace CareerTrack.Client.Models
{
    public class JobApplicationModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int ApplicationStatusId { get; set; }
        public DateTime DateApplied { get; set; }
        public string? Location { get; set; }
        public string? JobUrl { get; set; }
        public decimal? Salary { get; set; }
        public string? Notes { get; set; }
    }
}
