using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CareerCompass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIController : ControllerBase
    {
        private readonly HttpClient _http;

        public AIController(HttpClient http)
        {
            _http = http;
        }

        public class ResumeRequest
        {
            public string ResumeText { get; set; }
            public string JobInterests { get; set; }
        }

        [HttpPost("analyze")]
        public async Task<IActionResult> AnalyzeResume([FromBody] ResumeRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.ResumeText))
                return BadRequest("Resume text is required.");

            // 🔥 Put your API key inside appsettings.json, NOT in code
            string apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new {
                        role="user",
                        content=$"You are an expert career advisor, HR analyst, and résumé optimization specialist." +
                        $"Your task is to:\r\n1. Analyze the user's résumé.\r\n2. Compare it against the user's job interests or target role.\r\n3. Identify missing hard skills, soft skills, and technical skills.\r\n4. Identify weak areas or red flags in the résumé.\r\n5. Suggest improvements that will increase the user's chances of getting hired.\r\n6. Recommend additional certifications, portfolio projects, or courses relevant to the target role.\r\n7. Rewrite or enhance key résumé sections if needed.\r\n\r\nReturn your output in clear, structured sections." +
                        $"Here is a resume:\n{req.ResumeText}\n\nJob interests: {req.JobInterests}\n\n" +
                        $"Provide the analysis using the following structure:\r\n\r\n1. **Role Target Summary**\r\n   - A brief explanation of what the target job typically requires.\r\n   - How well the candidate currently aligns with it.\r\n\r\n2. **Key Strengths Found in the Resume**\r\n   - Bullet points of strong skills, experience, and achievements.\r\n\r\n3. **Missing Skills (High Priority)**\r\n   - Important skills the target position requires but the résumé lacks.\r\n\r\n4. **Missing Skills (Nice-to-Have)**\r\n   - Additional skills that will improve competitiveness.\r\n\r\n5. **Résumé Weaknesses / Red Flags**\r\n   - Gaps, lack of measurable results, unclear responsibilities, outdated tools, etc.\r\n\r\n6. **Suggestions for Immediate Improvement**\r\n   - Specific ways to rewrite or reorganize résumé sections.\r\n\r\n7. **Recommended Learning Path**\r\n   - Certifications, courses, or portfolio projects relevant to the job interest.\r\n\r\nBe specific, concise, and tailored only to the given résumé and job interest.\r\nDo NOT invent skills that are not present in the résumé.\r\nDo NOT hallucinate experience."
                    }
                }
            };

            string jsonBody = JsonSerializer.Serialize(body);
            var response = await _http.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(jsonBody, Encoding.UTF8, "application/json")
            );

            string result = await response.Content.ReadAsStringAsync();
            //get only the reply and not whole body
            var json = JsonNode.Parse(result);
            var reply = json?["choices"]?[0]?["message"]?["content"]?.ToString();

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("OpenAI Response: " + raw);

            return Ok(new { reply });
        }
    }
}