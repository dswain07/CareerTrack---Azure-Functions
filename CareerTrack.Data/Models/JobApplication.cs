using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CareerTrack.Data.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string JobTitle { get; set; } = string.Empty;

        [Required]
        public DateTime DateApplied { get; set; } = DateTime.Today;

        [StringLength(100)]
        public string? Location { get; set; }

        [Url]
        public string? JobUrl { get; set; }

        public decimal? Salary { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Relationship with Company
        [Required]
        public int CompanyId { get; set; }

        public Company? Company { get; set; }  

        // Relationship with ApplicationStatus
        [Required]
        public int ApplicationStatusId { get; set; }

        public ApplicationStatus? ApplicationStatus { get; set; } 

        // One application can have multiple interviews
        public ICollection<Interview> Interviews { get; set; } = new List<Interview>();
    }
}
