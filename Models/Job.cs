using System.ComponentModel.DataAnnotations;

namespace AIJobTracker.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "";

        [Required]
        public string Company { get; set; } = "";

        public string Location { get; set; } = "";

        [Required]
        public string Status { get; set; } = "";

        public DateTime AppliedDate { get; set; }

        // Job details
        [Url]
        public string? JobUrl { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SalaryMax { get; set; }

        public string? JobType { get; set; }

        public string? WorkMode { get; set; }

        public DateTime? Deadline { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        // User relationship
        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }

        // Status history
        public ICollection<JobStatusHistory> StatusHistory { get; set; }
            = new List<JobStatusHistory>();
    }
}