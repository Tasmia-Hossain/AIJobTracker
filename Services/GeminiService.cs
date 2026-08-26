using Google.GenAI;
using Google.GenAI.Types;

namespace AIJobTracker.Services
{
    public class GeminiService
    {
        private readonly Client _client;

        public GeminiService(IConfiguration configuration)
        {
            var apiKey = System.Environment.GetEnvironmentVariable("GEMINI_API_KEY");

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("GEMINI_API_KEY is not configured.");
            }

            _client = new Client(apiKey: apiKey);
        }

        public async Task<string> AnalyzeJobDescriptionAsync(string jobDescription)
        {
            var prompt = $"""
                Analyze the following job description for a software engineering candidate.

                Provide:
                1. A short summary
                2. Required technical skills
                3. Important keywords
                4. Experience requirements
                5. A simple recommendation for the candidate

                Job Description:
                {jobDescription}
                """;

            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.6-flash",
                contents: prompt
            );

            return response.Candidates?[0]?.Content?.Parts?[0]?.Text
                   ?? "No analysis was generated.";
        }
    }
}