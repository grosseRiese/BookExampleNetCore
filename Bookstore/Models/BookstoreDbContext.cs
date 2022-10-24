using Bookstore.Models.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bookstore.Models
{
    public class BookstoreDbContext:DbContext
    {
        protected readonly IConfiguration Configuration;
        public BookstoreDbContext(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           // connect to sql server with connection string from app settings
           options.UseSqlServer(Configuration.GetConnectionString("SqlCon"));
            
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}

/***
 * use SQL Server : Run the following command from the project root folder to install the EF Core database provider for SQL Server from NuGet:
 * dotnet add package Microsoft.EntityFrameworkCore.SqlServer
 * 
 * 
 * SQL Database from code with EF Core Migrations :
 * dotnet add package Microsoft.EntityFrameworkCore.Design
 * 
 * 
 * to use migration you need to install:
 * Microsoft.EntityFrameworkCore.Tools
 * **/