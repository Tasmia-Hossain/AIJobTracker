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

        public async Task<string> AnalyzeJobDescriptionAsync(
            string jobDescription,
            string? mySkills = null)
        {
            var skillsSection = string.IsNullOrWhiteSpace(mySkills)
                ? """
                  The candidate did not provide a personal skills list.
                  Do not invent candidate skills.
                  Focus on the skills and requirements mentioned in the job description.
                  """
                : $"""
                  Candidate's current skills:
                  {mySkills}

                  Compare the candidate's skills against the job requirements.
                  Clearly identify matching skills and missing or weak skills.
                  Do not assume the candidate has skills that are not listed.
                  """;

            var prompt = $"""
                You are an AI career assistant helping an entry-level software engineering candidate understand a job opportunity.

                Analyze the following job description carefully.

                {skillsSection}

                Return a clear, practical and structured analysis using Markdown.

                Use exactly these sections:

                ## 1. Job Summary
                Give a concise summary of the role and what the company is looking for.

                ## 2. Technical Skills
                List the programming languages, frameworks, databases, concepts and technical skills mentioned.

                ## 3. Tools & Technologies
                List specific tools, platforms, libraries, cloud technologies and development tools.

                ## 4. Soft Skills
                List communication, teamwork, leadership, problem-solving and other soft skills requested.

                ## 5. Education & Qualifications
                Mention degree, academic background, certifications and other qualifications required or preferred.

                ## 6. Experience Requirements
                Explain required years of experience, internship experience, project experience or entry-level expectations.

                ## 7. Key Responsibilities
                Summarize the main responsibilities of the role as concise bullet points.

                ## 8. Must-Have Skills
                List the most important skills that appear essential for the role.

                ## 9. Nice-to-Have Skills
                List skills that are preferred or beneficial but appear less critical.

                ## 10. Important Keywords
                Provide important ATS/resume keywords from the job description.

                ## 11. Skill Match
                If candidate skills were provided, divide the analysis into:
                - Strong matches
                - Partial matches
                - Missing or weak skills

                If candidate skills were not provided, explain that a personal skill comparison could not be calculated.

                ## 12. Preparation Plan
                Give a practical prioritized learning/preparation plan for this specific role.
                Focus on the most important gaps first.
                Do not recommend learning everything at once.

                ## 13. Application Recommendation
                Give a short recommendation:
                - Apply
                - Apply with preparation
                - Low priority

                Explain the recommendation in 2-4 sentences.

                Important rules:
                - Do not invent requirements that are not present in the job description.
                - Clearly distinguish required and preferred qualifications.
                - Keep the analysis practical for a junior/entry-level candidate.
                - Do not claim a candidate has a skill unless it appears in the candidate skills list.
                - Use concise bullet points.
                - Do not include generic motivational content.

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

        public async Task<string> MatchResumeWithJobAsync(
    string resumeText,
    string jobDescription)
        {
            var prompt = $"""
        You are an AI career assistant helping an entry-level
        software engineering candidate evaluate their resume
        against a specific job opportunity.

        Compare the candidate's resume with the job description.

        Return a clear, practical analysis using Markdown.

        Use exactly these sections:

        ## 1. Match Score
        Give an estimated match score from 0 to 100 based only
        on the evidence present in the resume and job description.

        Briefly explain the score.

        ## 2. Strong Matches
        List skills, technologies, qualifications and experience
        that clearly match the job requirements.

        ## 3. Partial Matches
        List areas where the resume shows some relevant knowledge
        or experience but does not fully match the requirement.

        ## 4. Missing or Weak Skills
        List important job requirements that are not clearly
        demonstrated in the resume.

        Only include requirements that actually appear in the
        job description.

        ## 5. Matched Keywords
        List important keywords from the job description that
        are also supported by the resume.

        ## 6. Skill Gaps
        Identify the most important technical or qualification
        gaps that the candidate should address.

        Prioritize the gaps instead of listing everything.

        ## 7. Preparation Suggestions
        Give a short practical preparation plan based specifically
        on the identified gaps.

        ## 8. Application Recommendation
        Choose one:
        - Strong match
        - Apply with preparation
        - Low priority

        Explain the recommendation in 2-4 sentences.

        Important rules:
        - Do not invent skills in the resume.
        - Do not invent requirements in the job description.
        - Only consider evidence explicitly present in the resume.
        - Clearly distinguish required and preferred qualifications.
        - Do not treat a missing skill as a weakness if the job
          description does not require or prefer it.
        - Focus on junior/entry-level relevance.
        - Keep the analysis concise and practical.
        - Do not provide generic motivational content.

        CANDIDATE RESUME:
        {resumeText}

        JOB DESCRIPTION:
        {jobDescription}
        """;

            var response = await _client.Models.GenerateContentAsync(
                model: "gemini-3.6-flash",
                contents: prompt
            );

            return response.Candidates?[0]?.Content?.Parts?[0]?.Text
                   ?? "No match analysis was generated.";
        }
    }
}