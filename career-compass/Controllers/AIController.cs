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
                        content=$"Here is a resume:\n{req.ResumeText}\n\nJob interests: {req.JobInterests}\n\nAnalyze missing skills and provide improvement suggestions."
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