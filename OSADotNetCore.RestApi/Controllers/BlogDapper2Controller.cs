using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSADotNetCore.ConsoleApp.Services;
using OSADotNetCore.RestApi.Db;
using OSADotNetCore.RestApi.Models;
using OSADotNetCore.Shared;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.Metadata;

namespace OSADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogDapper2Controller : ControllerBase
    {
        private readonly DapperService _dapperService = new DapperService(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "SELECT * FROM Blog";

            var lst = _dapperService.Query<BlogModel>(query);

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlogs(int id)
        {
            var item = FindById(id);
            if(item is null)
            {
                Console.WriteLine("No data found.");
                return NotFound("No data found.");
            }
            return Ok(item);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog)
        {
            string query = @"
                INSERT INTO [dbo].[Blog]
                    ([Title],[Author],[BlogContent])
                VALUES
                    (@Title,@Author,@BlogContent)";

            int result = _dapperService.Execute(query, blog);

            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBlog(int id, BlogModel blog)
        {
            var item = FindById(id);
            if(item is null)
            {
                return NotFound("No data found.");
            }

            blog.Id = id;
            string query = @"
                UPDATE [dbo].[Blog]
                SET 
                    [Title] = @Title,
                    [Author] = @Author,
                    [BlogContent] = @BlogContent
                WHERE Id = @Id";

            int result = _dapperService.Execute(query, blog);

            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            var item = FindById(id);
            if(item is null)
            {
                return NotFound("No data found.");
            }

            string conditions = string.Empty;
            if(!string.IsNullOrEmpty(blog.Title))
            {
                conditions += " [Title] = @Title, ";
            }

            if(!string.IsNullOrEmpty(blog.Author))
            {
                conditions += " [Author] = @Author, ";
            }

            if(!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += " [BlogContent] = @BlogContent, ";
            }

            if(conditions.Length == 0)
            {
                return NotFound("No data to update.");
            }

            conditions = conditions.Substring(0, conditions.Length - 2);
            blog.Id = id;

            string query = $@"
                UPDATE [dbo].[Blog]
                SET {conditions}
                WHERE Id = @Id";

            int result = _dapperService.Execute(query, blog);

            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            var item = FindById(id);
            if(item is null)
            {
                return NotFound("No data found.");
            }

            string query = @"Delete From [dbo].[Blog] WHERE Id = @Id";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, new BlogModel { Id = id });

            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            return Ok(message);
        }

        private BlogModel? FindById(int id)
        {
            string query = "SELECT * FROM Blog WHERE Id = @Id";
            var item = _dapperService.QueryFirstOrDefault<BlogModel>(query, new BlogModel { Id = id });
            return item;
        }
    }
}
