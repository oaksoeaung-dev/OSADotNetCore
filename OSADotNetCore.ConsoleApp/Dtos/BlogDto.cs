using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSADotNetCore.ConsoleApp.Dtos;

[Table("Tbl_Blog")]
public class BlogDto
{
    [Key]
    public int BlogId { get; set; } // database ထဲက table ထဲက column တွေနဲ့တူရမယ်
    public string BlogTitle { get; set; }
    public string BlogAuthor { get; set; }
    public string BlogContent { get; set; }
}
