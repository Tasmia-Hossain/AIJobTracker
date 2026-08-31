using AIJobTracker.Services;
using Markdig;
using Microsoft.AspNetCore.Mvc;

namespace AIJobTracker.Controllers
{
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
        public async Task<IActionResult> Analyze(string jobDescription)
        {
            if (string.IsNullOrWhiteSpace(jobDescription))
            {
                ViewBag.Error = "Please enter a job description.";
                return View();
            }

            try
            {
                var result = await _geminiService
                    .AnalyzeJobDescriptionAsync(jobDescription);

                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .Build();

                ViewBag.AnalysisHtml = Markdown.ToHtml(result, pipeline);
            }
            catch (Exception)
            {
                ViewBag.Error = "AI analysis failed. Please try again.";
            }

            ViewBag.JobDescription = jobDescription;

            return View();
        }
    }
}
