using AIJobTracker.Data;
using AIJobTracker.Models;
using AIJobTracker.Services;
using AIJobTracker.ViewModels;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AIJobTracker.Controllers
{
    [Authorize]
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly GeminiService _geminiService;

        public ResumeController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            GeminiService geminiService)
        {
            _context = context;
            _userManager = userManager;
            _geminiService = geminiService;
        }

        [HttpGet]
        public async Task<IActionResult> Match()
        {
            var userId = _userManager.GetUserId(User);

            var jobs = await _context.Jobs
                .Where(j => j.ApplicationUserId == userId)
                .OrderByDescending(j => j.AppliedDate)
                .AsNoTracking()
                .ToListAsync();

            return View(new ResumeMatchViewModel
            {
                Jobs = jobs
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Match(
            ResumeMatchViewModel model)
        {
            var userId = _userManager.GetUserId(User);

            var job = await _context.Jobs
                .FirstOrDefaultAsync(j =>
                    j.Id == model.JobId &&
                    j.ApplicationUserId == userId);

            if (job == null)
            {
                model.Error = "The selected job could not be found.";
            }
            else if (string.IsNullOrWhiteSpace(job.JobDescription))
            {
                model.Error =
                    "This job does not have a job description. " +
                    "Please add one before matching your resume.";
            }
            else if (string.IsNullOrWhiteSpace(model.ResumeText))
            {
                model.Error = "Please enter your resume text.";
            }
            else
            {
                try
                {
                    var result = await _geminiService
                        .MatchResumeWithJobAsync(
                            model.ResumeText,
                            job.JobDescription);

                    var pipeline = new MarkdownPipelineBuilder()
                        .UseAdvancedExtensions()
                        .Build();

                    model.AnalysisHtml = Markdown.ToHtml(
                        result,
                        pipeline);
                }
                catch (Exception)
                {
                    model.Error =
                        "Resume matching failed. Please try again.";
                }
            }

            model.Jobs = await _context.Jobs
                .Where(j => j.ApplicationUserId == userId)
                .OrderByDescending(j => j.AppliedDate)
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }
    }
}