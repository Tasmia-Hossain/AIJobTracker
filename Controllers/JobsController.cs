using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        public IActionResult Index(string search, string status)
        {
            var userId = _userManager.GetUserId(User);

            var jobs = _context.Jobs
                .Where(j => j.ApplicationUserId == userId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                jobs = jobs.Where(j =>
                    j.Title.Contains(search) ||
                    j.Company.Contains(search) ||
                    j.Location.Contains(search));
            }

            if (!string.IsNullOrEmpty(status))
            {
                jobs = jobs.Where(j => j.Status == status);
            }

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
            if (!ModelState.IsValid)
            {
                return View(job);
            }

            job.ApplicationUserId = _userManager.GetUserId(User);

            _context.Jobs.Add(job);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Details/5
        public IActionResult Details(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var job = _context.Jobs
                .FirstOrDefault(j => j.Id == id && j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Jobs/Edit/5
        public IActionResult Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var job = _context.Jobs
                .FirstOrDefault(j => j.Id == id && j.ApplicationUserId == userId);

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
            if (!ModelState.IsValid)
            {
                return View(job);
            }

            var userId = _userManager.GetUserId(User);

            var existingJob = _context.Jobs
                .FirstOrDefault(j => j.Id == job.Id &&
                                     j.ApplicationUserId == userId);

            if (existingJob == null)
            {
                return NotFound();
            }

            existingJob.Title = job.Title;
            existingJob.Company = job.Company;
            existingJob.Location = job.Location;
            existingJob.Status = job.Status;
            existingJob.AppliedDate = job.AppliedDate;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Jobs/Delete/5
        public IActionResult Delete(int id)
        {
            var userId = _userManager.GetUserId(User);

            var job = _context.Jobs
                .FirstOrDefault(j => j.Id == id &&
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
                .FirstOrDefault(j => j.Id == id &&
                                     j.ApplicationUserId == userId);

            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}