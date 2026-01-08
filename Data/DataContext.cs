using System.Data;
using EnglishCenter.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using EnglishCenter.Models;
namespace EnglishCenter.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Section> Sections{ get; set; }
    }
}
