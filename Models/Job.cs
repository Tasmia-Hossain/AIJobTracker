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

        public string? ApplicationUserId { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}