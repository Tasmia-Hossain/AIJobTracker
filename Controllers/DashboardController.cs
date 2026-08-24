using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AIJobTracker.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);

            var jobs = _context.Jobs
                .Where(j => j.ApplicationUserId == userId);

            var viewModel = new DashboardViewModel
            {
                TotalJobs = jobs.Count(),
                SavedJobs = jobs.Count(j => j.Status == "Saved"),
                AppliedJobs = jobs.Count(j => j.Status == "Applied"),
                InterviewJobs = jobs.Count(j => j.Status == "Interview"),
                RejectedJobs = jobs.Count(j => j.Status == "Rejected"),
                OfferJobs = jobs.Count(j => j.Status == "Offer"),
                WithdrawnJobs = jobs.Count(j => j.Status == "Withdrawn")
            };

            return View(viewModel);
        }
    }
}