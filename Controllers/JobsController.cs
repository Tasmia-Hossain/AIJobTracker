using AIJobTracker.Data;
using AIJobTracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace AIJobTracker.Controllers
{
    public class JobsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search, string status)
        {
            var jobs = _context.Jobs.AsQueryable();

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            if (!ModelState.IsValid)
            {
                return View(job);
            }

            _context.Jobs.Add(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var job = _context.Jobs.Find(id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }
        public IActionResult Edit(int id)
        {
            var job = _context.Jobs.Find(id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [HttpPost]
        public IActionResult Edit(Job job)
        {
            _context.Jobs.Update(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var job = _context.Jobs.Find(id);

            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var job = _context.Jobs.Find(id);

            if (job == null)
            {
                return NotFound();
            }

            _context.Jobs.Remove(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}