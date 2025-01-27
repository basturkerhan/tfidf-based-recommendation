using API.DialogRecommendation.Interfaces;
using API.DialogRecommendation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController(IDialogRecommender recommender) : ControllerBase
    {
        private readonly IDialogRecommender _recommender = recommender;

        [HttpGet]
        public IActionResult Get(string query)
        {
            _recommender.SetEntities(GetDialogs());
            var results = _recommender.CreateRecommendation(query, 3);

            return Ok(results.Select(r => r.Entity?.Answer));
        }


        private static List<Dialog> GetDialogs()
        {
            var content = System.IO.File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Dataset", "Question.json"));
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(content);
            int id = 0;
            return data!.Select(d =>
            {
                id++;
                return new Dialog()
                {
                    Id = id.ToString(),
                    Question = d.Key,
                    Answer = d.Value
                };
            }).ToList();
        }
    }
}
