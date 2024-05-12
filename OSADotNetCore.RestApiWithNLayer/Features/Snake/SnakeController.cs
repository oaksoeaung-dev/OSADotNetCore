using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace OSADotNetCore.RestApiWithNLayer.Features.Snake
{
    [Route("api/[controller]")]
    [ApiController]
    public class SnakeController : ControllerBase
    {
        private async Task<SnakeDetail> GetDataAsync()
        {
            string jsonStr = await System.IO.File.ReadAllTextAsync("snakes.json");
            var model = JsonConvert.DeserializeObject<SnakeDetail>(jsonStr);
            var a = model;
            return a;
        }

        [HttpGet]
        public async Task<IActionResult> GetSnakes()
        {
            var model = await GetDataAsync();
            return Ok(model.snakes.Select(s => new
            {
                Id = s.Id,
                MMName = s.MMName
            }).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSnake(int id)
        {
            var model = await GetDataAsync();
            return Ok(model.snakes.Where(s => s.Id == id));
        }
    }


    public class SnakeDetail
    {
        public Snake[] snakes { get; set; }
    }

    public class Snake
    {
        public int Id { get; set; }
        public string MMName { get; set; }
        public string EngName { get; set; }
        public string Detail { get; set; }
        public string IsPoison { get; set; }
        public string IsDanger { get; set; }
    }

}
