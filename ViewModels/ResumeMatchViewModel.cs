using System.ComponentModel.DataAnnotations;
using AIJobTracker.Models;

namespace AIJobTracker.ViewModels
{
    public class ResumeMatchViewModel
    {
        [Required]
        public int JobId { get; set; }

        public List<Job> Jobs { get; set; } = new();

        [Required]
        [StringLength(20000)]
        public string ResumeText { get; set; } = "";

        public string? AnalysisHtml { get; set; }

        public string? Error { get; set; }
    }
}