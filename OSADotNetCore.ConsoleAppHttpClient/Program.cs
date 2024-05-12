// See https://aka.ms/new-console-template for more information
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

string jsonStr = await File.ReadAllTextAsync("snakes.json");
var model = JsonConvert.DeserializeObject<MainDto>(jsonStr);

public class MainDto
{
    public Snake[] Snakes { get; set; }
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
