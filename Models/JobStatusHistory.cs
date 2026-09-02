using System.ComponentModel.DataAnnotations;

namespace AIJobTracker.Models
{
    public class JobStatusHistory
    {
        public int Id { get; set; }

        [Required]
        public int JobId { get; set; }

        public Job? Job { get; set; }

        [Required]
        public string OldStatus { get; set; } = "";

        [Required]
        public string NewStatus { get; set; } = "";

        public DateTime ChangedAt { get; set; }
    }
}