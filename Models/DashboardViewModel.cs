namespace AIJobTracker.Models
{
    public class DashboardViewModel
    {
        // Basic statistics
        public int TotalApplications { get; set; }

        public int SavedApplications { get; set; }

        public int AppliedApplications { get; set; }

        public int InterviewApplications { get; set; }

        public int OfferApplications { get; set; }

        public int RejectedApplications { get; set; }

        public int WithdrawnApplications { get; set; }

        // Analytics
        public double ApplicationRate { get; set; }

        public double InterviewRate { get; set; }

        public double OfferRate { get; set; }

        // Recent jobs
        public List<Job> RecentApplications { get; set; }
            = new();

        // Upcoming deadlines
        public List<Job> UpcomingDeadlines { get; set; }
            = new();

        // Monthly application activity
        public List<MonthlyApplicationData> MonthlyApplications { get; set; }
            = new();
    }

    public class MonthlyApplicationData
    {
        public string Month { get; set; } = "";

        public int Count { get; set; }
    }
}