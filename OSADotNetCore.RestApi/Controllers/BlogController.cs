using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OSADotNetCore.RestApi.Db;
using OSADotNetCore.RestApi.Models;

namespace OSADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly AppDbContext _context = new AppDbContext();

        [HttpGet]
        public IActionResult Read()
        {
            var blogs = _context.Blogs.ToList();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var blog = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if(blog is null)
            {
                return NotFound("No data found");
            }
            return Ok(blog);
        }

        [HttpPost]
        public IActionResult Create(BlogModel blog)
        {
            _context.Blogs.Add(blog);
            var result = _context.SaveChanges();
            string message = result > 0 ? "Saving Successful" : "Saving Failed";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if(item is null)
            {
                return NotFound("No data found");
            }
            item.Title = blog.Title;
            item.Author = blog.Author;
            item.BlogContent = blog.BlogContent;
            var result = _context.SaveChanges();
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, BlogModel blog)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if(item is null)
            {
                return NotFound("No data found");
            }
            if(!string.IsNullOrEmpty(blog.Title))
            {
                item.Title = blog.Title;
            }
            if(!string.IsNullOrEmpty(blog.Author))
            {
                item.Author = blog.Author;
            }
            if(!string.IsNullOrEmpty(blog.BlogContent))
            {
                item.BlogContent = blog.BlogContent;
            }
            var result = _context.SaveChanges();
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);
            if(item is null)
            {
                return NotFound("No data found");
            }

            _context.Blogs.Remove(item);
            var result = _context.SaveChanges();
            string message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            return Ok(message);
        }
    }
}
