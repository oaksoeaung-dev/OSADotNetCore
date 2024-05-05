using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OSADotNetCore.ConsoleApp.Services;
using OSADotNetCore.RestApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace OSADotNetCore.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogAdoDotNetController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetBlogs()
        {
            string query = "SELECT * FROM Blog";

            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();

            //List<BlogModel> lst = new List<BlogModel>();
            //foreach (DataRow dr in dt.Rows)
            //{
            //    //BlogModel blog = new BlogModel();
            //    //blog.BlogId = Convert.ToInt32(dr["BlogId"]);
            //    //blog.BlogTitle = Convert.ToString(dr["BlogTitle"]);
            //    //blog.BlogAuthor = Convert.ToString(dr["BlogAuthor"]);
            //    //blog.BlogContent = Convert.ToString(dr["BlogContent"]);

            //    BlogModel blog = new BlogModel
            //    {
            //        BlogId = Convert.ToInt32(dr["BlogId"]),
            //        BlogTitle = Convert.ToString(dr["BlogTitle"]),
            //        BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
            //        BlogContent = Convert.ToString(dr["BlogContent"])
            //    };
            //    lst.Add(blog);
            //}

            List<BlogModel> lst = dt.AsEnumerable().Select(dr => new BlogModel
            {
                Id = Convert.ToInt32(dr["Id"]),
                Title = Convert.ToString(dr["Title"]),
                Author = Convert.ToString(dr["Author"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            }).ToList();

            return Ok(lst);
        }

        [HttpGet("{id}")]
        public IActionResult GetBlog(int id)
        {
            string query = "SELECT * FROM Blog where Id = @Id";

            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();

            if(dt.Rows.Count == 0)
            {
                return NotFound("No data found.");
            }

            DataRow dr = dt.Rows[0];
            var item = new BlogModel
            {
                Id = Convert.ToInt32(dr["Id"]),
                Title = Convert.ToString(dr["Title"]),
                Author = Convert.ToString(dr["Author"]),
                BlogContent = Convert.ToString(dr["BlogContent"])
            };

            return Ok(dt);
        }

        [HttpPost]
        public IActionResult CreateBlog(BlogModel blog)
        {
            string query = @"
                INSERT INTO [dbo].[Blog]
                    ([Title],[Author],[BlogContent])
                VALUES
                    (@Title,@Author,@BlogContent)";

            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Title", blog.Title);
            cmd.Parameters.AddWithValue("@Author", blog.Author);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);
            int result = cmd.ExecuteNonQuery();

            connection.Close();

            string message = result > 0 ? "Saving Successful." : "Saving Failed.";
            //return StatusCode(500, message);
            return Ok(message);
        }

        [HttpPut("{id}")]
        public IActionResult PutBlog(int id, BlogModel blog)
        {
            string getQuery = "SELECT COUNT(*) FROM Blog WHERE Id = @Id";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            SqlCommand checkCmd = new SqlCommand(getQuery, connection);
            checkCmd.Parameters.AddWithValue("@Id", id);
            var count = (int)checkCmd.ExecuteScalar();
            if(count == 0) return NotFound();

            string updateQuery = @"UPDATE [dbo].[Blog]
                           SET [Title] = @Title,
                               [Author] = @Author,
                               [BlogContent] = @BlogContent
                           WHERE Id = @Id";

            SqlCommand cmd = new SqlCommand(updateQuery, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", blog.Title);
            cmd.Parameters.AddWithValue("@Author", blog.Author);
            cmd.Parameters.AddWithValue("@BlogContent", blog.BlogContent);

            int result = cmd.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Updating Successful." : "Updating Failed.";
            return Ok(message);
        }

        [HttpPatch("{id}")]
        public IActionResult PatchBlog(int id, BlogModel blog)
        {
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();
            string query = "SELECT * FROM Blog WHERE Id = @Id";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);
            var count = (int)cmd.ExecuteScalar();
            if(count == 0) return NotFound();
            SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dataAdapter.Fill(dt);
            List<BlogModel> lst = new List<BlogModel>();
            if(dt.Rows.Count == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            DataRow row = dt.Rows[0];

            BlogModel item = new BlogModel
            {
                Id = Convert.ToInt32(row["Id"]),
                Title = Convert.ToString(row["Title"]),
                Author = Convert.ToString(row["Author"]),
                BlogContent = Convert.ToString(row["BlogContent"])
            };
            lst.Add(item);
            string conditions = "";
            List<SqlParameter> parameters = new List<SqlParameter>();

            #region Patch Validation Conditions

            if(!string.IsNullOrEmpty(blog.Title))
            {
                conditions += " [Title] = @Title, ";
                parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar) { Value = blog.Title });
                item.Title = blog.Title;
            }

            if(!string.IsNullOrEmpty(blog.Author))
            {
                conditions += " [Author] = @Author, ";
                parameters.Add(new SqlParameter("@Author", SqlDbType.NVarChar) { Value = blog.Author });
                item.Author = blog.Author;
            }

            if(!string.IsNullOrEmpty(blog.BlogContent))
            {
                conditions += " [BlogContent] = @BlogContent, ";
                parameters.Add(new SqlParameter("@BlogContent", SqlDbType.NVarChar) { Value = blog.BlogContent });
                item.BlogContent = blog.BlogContent;
            }

            if(conditions.Length == 0)
            {
                var response = new { IsSuccess = false, Message = "No data found." };
                return NotFound(response);
            }

            #endregion

            conditions = conditions.TrimEnd(',', ' ');
            query = $@"UPDATE [dbo].[Blog] SET {conditions} WHERE Id = @Id";

            using SqlCommand cmd2 = new SqlCommand(query, connection);
            cmd2.Parameters.AddWithValue("@Id", id);
            cmd2.Parameters.AddRange(parameters.ToArray());

            int result = cmd2.ExecuteNonQuery();
            connection.Close();
            string message = result > 0 ? "Patch Updating Successful." : "Patch Updating Failed.";
            return Ok(message);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBlog(int id)
        {
            string query = @"DELETE FROM [dbo].[Blog]
                           WHERE Id = @Id";
            SqlConnection connection = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            int result = cmd.ExecuteNonQuery();
            connection.Close();

            string message = result > 0 ? "Deleting Successful." : "Deleting Failed.";
            return Ok(message);
        }
    }
}
