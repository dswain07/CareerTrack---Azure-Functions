using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CareerTrack.Data.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Industry { get; set; } 

        [StringLength(100)]
        public string? Location { get; set; } 

        [Url]
        public string? Website { get; set; } 

        public ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

        // One company can have multiple contacts
        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    }
}
