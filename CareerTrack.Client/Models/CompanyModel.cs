using System;
using System.Collections.Generic;
using System.Text;

namespace CareerTrack.Client.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Industry { get; set; }
        public string? Location { get; set; }
    }
}
