using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIJobTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var viewModel = new DashboardViewModel
            {
                TotalJobs = _context.Jobs.Count(),
                SavedJobs = _context.Jobs.Count(j => j.Status == "Saved"),
                AppliedJobs = _context.Jobs.Count(j => j.Status == "Applied"),
                InterviewJobs = _context.Jobs.Count(j => j.Status == "Interview"),
                RejectedJobs = _context.Jobs.Count(j => j.Status == "Rejected"),
                OfferJobs = _context.Jobs.Count(j => j.Status == "Offer"),
                WithdrawnJobs = _context.Jobs.Count(j => j.Status == "Withdrawn")
            };

            return View(viewModel);
        }
    }
}