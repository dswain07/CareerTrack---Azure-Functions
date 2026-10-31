namespace CareerTrack.Api.ServiceModels
{
    public class CompanyModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Industry { get; set; }

        public string? Location { get; set; }
    }
}
