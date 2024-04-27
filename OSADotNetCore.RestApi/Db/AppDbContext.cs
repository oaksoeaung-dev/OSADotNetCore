using Microsoft.EntityFrameworkCore;
using OSADotNetCore.ConsoleApp.Services;
using OSADotNetCore.RestApi.Models;

namespace OSADotNetCore.RestApi.Db
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
        }

        public DbSet<BlogModel> Blogs { get; set; }
    }
}
