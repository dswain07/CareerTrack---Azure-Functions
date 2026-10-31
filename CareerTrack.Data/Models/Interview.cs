using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CareerTrack.Data.Models
{
    public class Interview
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string InterviewType { get; set; } = string.Empty;

        [Required]
        public DateTime InterviewDate { get; set; } = DateTime.Now;

        [StringLength(100)]
        public string? Location { get; set; }

        [StringLength(50)]
        public string? Outcome { get; set; }

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Foreign key to JobApplication
        public int JobApplicationId { get; set; }

        // Navigation property
        public JobApplication? JobApplication { get; set; } 
    }
}
