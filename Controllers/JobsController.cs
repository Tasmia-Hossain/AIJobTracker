using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIJobTracker.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public JobsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Jobs
        public IActionResult Index(
            string? search,
            string? status,
            string? jobType,
            string? workMode,
            decimal? minSalary,
            decimal? maxSalary,
            DateTime? appliedFrom,
            DateTime? appliedTo,
            string? deadline,
            string? sortBy)
        {
            var userId = _userManager.GetUserId(User);

            var jobs = _context.Jobs
                .Where(j => j.ApplicationUserId == userId)
                .AsNoTracking()
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                jobs = jobs.Where(j =>
                    j.Title.Contains(search) ||
                    j.Company.Contains(search) ||
                    j.Location.Contains(search));
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                jobs = jobs.Where(j => j.Status == status);
            }

            // Job Type filter
            if (!string.IsNullOrWhiteSpace(jobType))
            {
                jobs = jobs.Where(j => j.JobType == jobType);
            }

            // Work Mode filter
            if (!string.IsNullOrWhiteSpace(workMode))
            {
                jobs = jobs.Where(j => j.WorkMode == workMode);
            }

            // Minimum salary
            if (minSalary.HasValue)
            {
                jobs = jobs.Where(j =>
                    j.SalaryMax.HasValue &&
                    j.SalaryMax.Value >= minSalary.Value);
            }

            // Maximum salary
            if (maxSalary.HasValue)
            {
                jobs = jobs.Where(j =>
                    j.SalaryMin.HasValue &&
                    j.SalaryMin.Value <= maxSalary.Value);
            }

            // Applied date - From
            if (appliedFrom.HasValue)
            {
                var fromDate = appliedFrom.Value.Date;

                jobs = jobs.Where(j =>
                    j.AppliedDate >= fromDate);
            }

            // Applied date - To
            if (appliedTo.HasValue)
            {
                var toDate = appliedTo.Value.Date.AddDays(1);

                jobs = jobs.Where(j =>
                    j.AppliedDate < toDate);
            }

            // Deadline filter
            if (!string.IsNullOrWhiteSpace(deadline))
            {
                var today = DateTime.Today;

                switch (deadline)
                {
                    case "upcoming":
                        jobs = jobs.Where(j =>
                            j.Deadline.HasValue &&
                            j.Deadline.Value.Date >= today);
                        break;

                    case "overdue":
                        jobs = jobs.Where(j =>
                            j.Deadline.HasValue &&
                            j.Deadline.Value.Date < today);
                        break;

                    case "no-deadline":
                        jobs = jobs.Where(j =>
                            !j.Deadline.HasValue);
                        break;
                }
            }

            // Sorting
            jobs = sortBy switch
            {
                "title-asc" =>
                    jobs.OrderBy(j => j.Title),

                "title-desc" =>
                    jobs.OrderByDescending(j => j.Title),

                "company-asc" =>
                    jobs.OrderBy(j => j.Company),

                "company-desc" =>
                    jobs.OrderByDescending(j => j.Company),

                "oldest" =>
                    jobs.OrderBy(j => j.AppliedDate),

                "deadline-asc" =>
                    jobs.OrderBy(j => j.Deadline == null)
                        .ThenBy(j => j.Deadline),

                "deadline-desc" =>
                    jobs.OrderByDescending(j => j.Deadline),

                "salary-high" =>
                    jobs.OrderByDescending(j => j.SalaryMax),

                "salary-low" =>
                    jobs.OrderBy(j => j.SalaryMin),

                _ =>
                    jobs.OrderByDescending(j => j.AppliedDate)
            };

            // Keep filter values for the view
            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.JobType = jobType;
            ViewBag.WorkMode = workMode;
            ViewBag.MinSalary = minSalary;
            ViewBag.MaxSalary = maxSalary;
            ViewBag.AppliedFrom = appliedFrom?.ToString("yyyy-MM-dd");
            ViewBag.AppliedTo = appliedTo?.ToString("yyyy-MM-dd");
            ViewBag.Deadline = deadline;
            ViewBag.SortBy = sortBy;

            return View(jobs.ToList());
        }

        // GET: Jobs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jobs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Job job)
        {
            ValidateSalaryRange(job);

            if (!ModelState.IsValid)
            {
                return View(job);
            }

            var userId = _userManager.GetUserId(User);

            job.ApplicationUserId = userId;

            _context.Jobs.Add(job);
            _context.SaveChanges();

            // Initial status history
            var history = new JobStatusHistory
            {
                JobId = job.Id,
                OldStatus = "Created",
                NewStatus = job.Status,
                ChangedAt = DateTime.Now
            };

            _context.JobStatusHistories.Add(history);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Details/5
        public IActionResult Details(int id)
        {
            var userId = _userManager.GetUserId(User);

            var job = _context.Jobs
                .Include(j => j.StatusHistory
                    .OrderByDescending(h => h.ChangedAt))
                .FirstOrDefault(j =>
                    j.Id == id &&
                    j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public IActionResult Edit(int id)
        {
            var userId = _userManager.GetUserId(User);

            var job = _context.Jobs
                .FirstOrDefault(j =>
                    j.Id == id &&
                    j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            ValidateSalaryRange(job);

            if (!ModelState.IsValid)
            {
                return View(job);
            }

            var userId = _userManager.GetUserId(User);

            var existingJob = _context.Jobs
                .FirstOrDefault(j =>
                    j.Id == job.Id &&
                    j.ApplicationUserId == userId);

            if (existingJob == null)
            {
                return NotFound();
            }

            var oldStatus = existingJob.Status;

            existingJob.Title = job.Title;
            existingJob.Company = job.Company;
            existingJob.Location = job.Location;
            existingJob.Status = job.Status;
            existingJob.AppliedDate = job.AppliedDate;

            existingJob.JobUrl = job.JobUrl;
            existingJob.SalaryMin = job.SalaryMin;
            existingJob.SalaryMax = job.SalaryMax;
            existingJob.JobType = job.JobType;
            existingJob.WorkMode = job.WorkMode;
            existingJob.Deadline = job.Deadline;
            existingJob.Notes = job.Notes;

            // Create history only when status actually changes
            if (oldStatus != job.Status)
            {
                var history = new JobStatusHistory
                {
                    JobId = existingJob.Id,
                    OldStatus = oldStatus,
                    NewStatus = job.Status,
                    ChangedAt = DateTime.Now
                };

                _context.JobStatusHistories.Add(history);
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = job.Id });
        }

        // GET: Jobs/Delete/5
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var job = _context.Jobs
                .FirstOrDefault(j =>
                    j.Id == id &&
                    j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Jobs/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);

            var job = _context.Jobs
                .FirstOrDefault(j =>
                    j.Id == id &&
                    j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Validate salary range
        private void ValidateSalaryRange(Job job)
        {
            if (job.SalaryMin.HasValue &&
                job.SalaryMax.HasValue &&
                job.SalaryMin > job.SalaryMax)
            {
                ModelState.AddModelError(
                    "",
                    "Minimum salary cannot be greater than maximum salary.");
            }
        }
    }
}