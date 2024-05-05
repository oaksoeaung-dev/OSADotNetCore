using DotNetTrainingBatch4.Shared;
using Microsoft.AspNetCore.Mvc;
using OSADotNetCore.ConsoleApp.Services;
using OSADotNetCore.RestApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DotNetTrainingBatch4.RestApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogAdoDotNet2Controller : ControllerBase
{
    private readonly AdoDotNetService _adoDotNetService =
        new AdoDotNetService(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);

    [HttpGet]
    public IActionResult GetBlogs()
    {
        string query = "SELECT * FROM Blog";
        var lst = _adoDotNetService.Query<BlogModel>(query);

        return Ok(lst);
    }

    [HttpGet("{id}")]
    public IActionResult GetBlog(int id)
    {
        string query = "SELECT * FROM Blog WHERE Id = @Id";

        var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query, new AdoDotNetParameter("@Id", id));

        if(item is null)
        {
            return NotFound("No data found.");
        }

        return Ok(item);
    }

    [HttpPost]
    public IActionResult CreateBlog(BlogModel blog)
    {
        string query = @"INSERT INTO [dbo].[Blog]
           ([Title]
           ,[Author]
           ,[BlogContent])
     VALUES
           (@Title
           ,@Author       
           ,@BlogContent)";

        int result = _adoDotNetService.Execute(query,
            new AdoDotNetParameter("@Title", blog.Title),
            new AdoDotNetParameter("@Author", blog.Author),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent)
        );

        string message = result > 0 ? "Saving Successful." : "Saving Failed.";
        //return StatusCode(500, message);
        return Ok(message);
    }

    [HttpPut("{id}")]
    public IActionResult PutBlog(int id, BlogModel blog)
    {
        var item = FindById(id);
        if(item == null)
        {
            return NotFound("No Data Found.");
        }

        string query = @"
            UPDATE [dbo].[Blog]
            SET 
                [Title] = @Title,
                [Author] = @Author,
                [BlogContent] = @BlogContent
            WHERE Id = @Id";

        int result = _adoDotNetService.Execute(query,
            new AdoDotNetParameter("@Id", id),
            new AdoDotNetParameter("@Title", blog.Title),
            new AdoDotNetParameter("@Author", blog.Author),
            new AdoDotNetParameter("@BlogContent", blog.BlogContent));

        string message = result > 0 ? "Updating Successful." : "Updating Failed.";
        return Ok(message);
    }

    [HttpPatch("{id}")]
    public IActionResult PatchBlog(int id, BlogModel blog)
    {
        List<AdoDotNetParameter> lst = new List<AdoDotNetParameter>();
        string conditions = string.Empty;
        if(!string.IsNullOrEmpty(blog.Title))
        {
            conditions += " [Title] = @Title, ";
            lst.Add(new AdoDotNetParameter("@Title", blog.Title));
        }

        if(!string.IsNullOrEmpty(blog.Author))
        {
            conditions += " [Author] = @Author, ";
            lst.Add(new AdoDotNetParameter("@Author", blog.Author));
        }

        if(!string.IsNullOrEmpty(blog.BlogContent))
        {
            conditions += " [BlogContent] = @BlogContent, ";
            lst.Add(new AdoDotNetParameter("@BlogContent", blog.BlogContent));
        }

        if(conditions.Length == 0)
        {
            var response = new { IsSuccess = false, Message = "No data found." };
            return NotFound(response);
        }
        lst.Add(new AdoDotNetParameter("@Id", id));

        conditions = conditions.Substring(0, conditions.Length - 2);
        string query = $@"UPDATE [dbo].[Blog] SET {conditions} WHERE Id = @Id";

        int result = _adoDotNetService.Execute(query,
            lst.ToArray()
        );

        string message = result > 0 ? "Updating Successful." : "Updating Failed.";
        return Ok(message);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBlog(int id)
    {
        var item = FindById(id);
        if(item == null)
        {
            return NotFound("No Data Found.");
        }

        string query = "DELETE FROM[dbo].[Blog] WHERE Id = @Id";

        int result = _adoDotNetService.Execute(query, new AdoDotNetParameter("@Id", id));


        string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
        return Ok(message);
    }

    private BlogModel FindById(int id)
    {
        string query = "SELECT * FROM Blog where Id = @Id";


        var item = _adoDotNetService.QueryFirstOrDefault<BlogModel>(query, new AdoDotNetParameter("@Id", id));

        if(item is null)
        {
            return null;
        }

        return item;
    }
}