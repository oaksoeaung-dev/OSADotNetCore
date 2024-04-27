using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSADotNetCore.RestApi.Models;

[Table("Blog")]
public class BlogModel
{
    [Key]
    public int Id { get; set; } // database ထဲက table ထဲက column တွေနဲ့တူရမယ်
    public string? Title { get; set; }
    public string? BlogContent { get; set; }
    public string? Author { get; set; }
}
