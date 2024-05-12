namespace OSADotNetCore.RestApiWithNLayer.Models;

[Table("Blog")]
public class BlogModel
{
    [Key]
    public int Id { get; set; } // database ထဲက table ထဲက column တွေနဲ့တူရမယ်
    public string? Title { get; set; }
    public string? BlogContent { get; set; }
    public string? Author { get; set; }
}
