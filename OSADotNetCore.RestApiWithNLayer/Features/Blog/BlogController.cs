using Microsoft.EntityFrameworkCore;

namespace OSADotNetCore.RestApiWithNLayer.Features.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BL_Blog _blBlog;

        public BlogController()
        {
            _blBlog = new BL_Blog();
        }

        [HttpGet]
        public IActionResult Read()
        {
            var blogs = _blBlog.GetBlogs();
            return Ok(blogs);
        }

        [HttpGet("{id}")]
        public IActionResult Edit(int id)
        {
            var blog = _blBlog.GetBlog(id);
            if(blog is null)
            {
                return NotFound("No data found");
            }
            return Ok(blog);
        }

        [HttpPost]
        public IActionResult Create(BlogModel blog)
        {
            var result = _blBlog.CreateBlog(blog);

            string message = result > 0 ? "Saving Successful" : "Saving Failed";
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, BlogModel blog)
        {
            var item = _blBlog.GetBlog(id);
            if(item is null)
            {
                return NotFound("No data found");
            }
            var result = _blBlog.UpdateBlog(id, blog);
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, BlogModel blog)
        {
            //var item = _context.Blogs.FirstOrDefault(x => x.Id == id);
            //if(item is null)
            //{
            //    return NotFound("No data found");
            //}
            //if(!string.IsNullOrEmpty(blog.Title))
            //{
            //    item.Title = blog.Title;
            //}
            //if(!string.IsNullOrEmpty(blog.Author))
            //{
            //    item.Author = blog.Author;
            //}
            //if(!string.IsNullOrEmpty(blog.BlogContent))
            //{
            //    item.BlogContent = blog.BlogContent;
            //}
            //var result = _context.SaveChanges();
            //string message = result > 0 ? "Updating Successful" : "Updating Failed";
            var item = _blBlog.GetBlog(id);
            if(item is null)
            {
                return NotFound("No data found");
            }
            var result = _blBlog.PatchBlog(id, blog);
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _blBlog.DeleteBlog(id);
            string message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            return Ok(message);
        }
    }
}
