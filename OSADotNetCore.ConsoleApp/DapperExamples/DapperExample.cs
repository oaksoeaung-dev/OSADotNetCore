using Dapper;
using OSADotNetCore.ConsoleApp.Dtos;
using OSADotNetCore.ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace OSADotNetCore.ConsoleApp.DapperExamples
{
    internal class DapperExample
    {
        public void Run()
        {
            Read();
            Edit(1);
            Edit(22);
            //Create("title", "author", "content");
            //Update(1002, "title2", "author2", "content2");
            Delete(1002);
        }

        public void Read()
        {
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            List<BlogDto> lst = db.Query<BlogDto>("SELECT * FROM tbl_blog").ToList();

            foreach(BlogDto item in lst)
            {
                Console.WriteLine(item.BlogId);
                Console.WriteLine(item.BlogTitle);
                Console.WriteLine(item.BlogAuthor);
                Console.WriteLine(item.BlogContent);
                Console.WriteLine("------------------------------------");
            }
        }

        public void Edit(int id)
        {
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            var item = db.Query("SELECT * FROM tbl_blog WHERE BlogId = @BlogId", new BlogDto { BlogId = id }).FirstOrDefault();
            if(item is null)
            {
                Console.WriteLine("No data found.");
                return;
            }

            Console.WriteLine(item.BlogId);
            Console.WriteLine(item.BlogTitle);
            Console.WriteLine(item.BlogAuthor);
            Console.WriteLine(item.BlogContent);
            Console.WriteLine("------------------------------------");
        }

        private void Create(string title, string author, string content)
        {
            var item = new BlogDto
            {
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content,
            };
            string query = @"
                INSERT INTO [dbo].[Tbl_Blog]
                       ([BlogTitle]
                       ,[BlogAuthor]
                       ,[BlogContent])
                VALUES
                       (@BlogTitle,
                       @BlogAuthor,
                       @BlogContent)
            ";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, item);
            string message = result > 0 ? "Saving Successful" : "Saving Failed";
            Console.WriteLine(message);
        }

        private void Update(int id, string title, string author, string content)
        {
            var item = new BlogDto
            {
                BlogId = id,
                BlogTitle = title,
                BlogAuthor = author,
                BlogContent = content,
            };
            string query = @"
                UPDATE [dbo].[Tbl_Blog]
                   SET [BlogTitle] = @BlogTitle
                      ,[BlogAuthor] = @BlogAuthor
                      ,[BlogContent] = @BlogContent
                WHERE [BlogId] = @BlogId
            ";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, item);
            string message = result > 0 ? "Updating Successful" : "Updating Failed";
            Console.WriteLine(message);
        }

        private void Delete(int id)
        {
            var item = new BlogDto
            {
                BlogId = id,
            };

            string query = @"
                DELETE FROM Tbl_Blog
                WHERE [BlogId] = @BlogId
            ";
            using IDbConnection db = new SqlConnection(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
            int result = db.Execute(query, item);
            string message = result > 0 ? "Deleting Successful" : "Deleting Failed";
            Console.WriteLine(message);
        }
    }
}
