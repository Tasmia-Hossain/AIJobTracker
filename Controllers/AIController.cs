using AIJobTracker.Services;
using Markdig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIJobTracker.Controllers
{
    [Authorize]
    public class AIController : Controller
    {
        private readonly GeminiService _geminiService;

        public AIController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        [HttpGet]
        public IActionResult Analyze()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Analyze(
            string jobDescription,
            string? mySkills)
        {
            ViewBag.JobDescription = jobDescription;
            ViewBag.MySkills = mySkills;

            if (string.IsNullOrWhiteSpace(jobDescription))
            {
                ViewBag.Error = "Please enter a job description.";
                return View();
            }

            try
            {
                var result = await _geminiService
                    .AnalyzeJobDescriptionAsync(
                        jobDescription,
                        mySkills);

                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .Build();

                ViewBag.AnalysisHtml = Markdown.ToHtml(
                    result,
                    pipeline);
            }
            catch (Exception)
            {
                ViewBag.Error =
                    "AI analysis failed. Please try again.";
            }

            return View();
        }
    }
}