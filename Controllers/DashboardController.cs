using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                .Where(j => j.ApplicationUserId == userId)
                .AsNoTracking()
                .ToList();

            var total = jobs.Count;

            var model = new DashboardViewModel
            {
                TotalApplications = total,

                SavedApplications =
                    jobs.Count(j => j.Status == "Saved"),

                AppliedApplications =
                    jobs.Count(j => j.Status == "Applied"),

                InterviewApplications =
                    jobs.Count(j => j.Status == "Interview"),

                OfferApplications =
                    jobs.Count(j => j.Status == "Offer"),

                RejectedApplications =
                    jobs.Count(j => j.Status == "Rejected"),

                WithdrawnApplications =
                    jobs.Count(j => j.Status == "Withdrawn"),

                RecentApplications = jobs
                    .OrderByDescending(j => j.AppliedDate)
                    .Take(5)
                    .ToList(),

                UpcomingDeadlines = jobs
                    .Where(j =>
                        j.Deadline.HasValue &&
                        j.Deadline.Value.Date >= DateTime.Today)
                    .OrderBy(j => j.Deadline)
                    .Take(5)
                    .ToList()
            };

            model.ApplicationRate = total > 0
                ? Math.Round(
                    (double)model.AppliedApplications / total * 100,
                    1)
                : 0;

            model.InterviewRate = total > 0
                ? Math.Round(
                    (double)model.InterviewApplications / total * 100,
                    1)
                : 0;

            model.OfferRate = total > 0
                ? Math.Round(
                    (double)model.OfferApplications / total * 100,
                    1)
                : 0;

            model.MonthlyApplications = jobs
                .GroupBy(j => new
                {
                    j.AppliedDate.Year,
                    j.AppliedDate.Month
                })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .TakeLast(6)
                .Select(g => new MonthlyApplicationData
                {
                    Month = new DateTime(
                        g.Key.Year,
                        g.Key.Month,
                        1).ToString("MMM yyyy"),

                    Count = g.Count()
                })
                .ToList();

            return View(model);
        }
    }
}