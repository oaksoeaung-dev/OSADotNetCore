namespace OSADotNetCore.RestApiWithNLayer.Features.Blog
{
    public class DA_Blog
    {
        private readonly AppDbContext _context;

        public DA_Blog()
        {
            _context = new AppDbContext();
        }

        public List<BlogModel> GetBlogs()
        {
            var blogs = _context.Blogs.ToList();
            return blogs;
        }

        public BlogModel GetBlog(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);
            return item;
        }

        public int CreateBlog(BlogModel requestModel)
        {
            _context.Blogs.Add(requestModel);
            var result = _context.SaveChanges();
            return result;
        }

        public int UpdateBlog(int id, BlogModel requestModel)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);

            if(item is null) return 0;

            item.Title = requestModel.Title;
            item.Author = requestModel.Author;
            item.BlogContent = requestModel.BlogContent;

            var result = _context.SaveChanges();
            return result;
        }

        public int PatchBlog(int id, BlogModel requestModel)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);

            if(item is null) return 0;

            if(!string.IsNullOrEmpty(requestModel.Title))
            {
                item.Title = requestModel.Title;
            }

            if(!string.IsNullOrEmpty(requestModel.Author))
            {
                item.Author = requestModel.Author;
            }

            if(!string.IsNullOrEmpty(requestModel.BlogContent))
            {
                item.BlogContent = requestModel.BlogContent;
            }

            var result = _context.SaveChanges();
            return result;
        }

        public int DeleteBlog(int id)
        {
            var item = _context.Blogs.FirstOrDefault(x => x.Id == id);

            if(item is null) return 0;

            _context.Blogs.Remove(item);
            var result = _context.SaveChanges();
            return result;
        }
    }
}
