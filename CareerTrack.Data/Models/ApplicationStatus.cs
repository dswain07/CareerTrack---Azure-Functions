using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CareerTrack.Data.Models
{
    public class ApplicationStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        // One status can belong to many job applications
        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
    }
}
