namespace AIJobTracker.Models
{
    public class DashboardViewModel
    {
        public int TotalJobs { get; set; }
        public int SavedJobs { get; set; }
        public int AppliedJobs { get; set; }
        public int InterviewJobs { get; set; }
        public int RejectedJobs { get; set; }
        public int OfferJobs { get; set; }
        public int WithdrawnJobs { get; set; }
    }
}