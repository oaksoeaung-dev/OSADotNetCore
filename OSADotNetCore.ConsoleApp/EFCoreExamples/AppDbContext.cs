using Microsoft.EntityFrameworkCore;
using OSADotNetCore.ConsoleApp.Dtos;
using OSADotNetCore.ConsoleApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSADotNetCore.ConsoleApp.EFCoreExamples
{
    internal class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionStrings.sqlConnectionStringBuilder.ConnectionString);
        }

        // သုံးမယ့် table ကိုကြေညာပေးရတယ်
        public DbSet<BlogDto> Blogs { get; set; }
    }
}
