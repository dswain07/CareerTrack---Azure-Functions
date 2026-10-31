using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CareerTrack.Data.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? JobTitle { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }

        // Foreign key to Company
        public int CompanyId { get; set; }

        // Navigation property
        public Company Company { get; set; } = null!;
    }
}
