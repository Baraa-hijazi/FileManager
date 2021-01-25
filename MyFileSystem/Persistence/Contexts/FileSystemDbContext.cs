using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyFileSystem.Entities;
using System.IO;

namespace MyFileSystem.Persistence
{
    public class FileSystemDbContext : DbContext
    {
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Entities.File> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) { }

        public FileSystemDbContext(DbContextOptions<FileSystemDbContext> Options) : base(Options)
        { 
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build(); 
        }

        public FileSystemDbContext() { }

        public class Fix : IDesignTimeDbContextFactory<FileSystemDbContext>
        {
            public FileSystemDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<FileSystemDbContext>();
                optionsBuilder.UseSqlServer("Server=localhost;Database=MyFileSystemDatabase;Trusted_Connection=True;");
                return new FileSystemDbContext(optionsBuilder.Options);
            }
        }
    }
}
