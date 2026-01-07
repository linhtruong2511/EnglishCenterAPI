using System.Data;
using EnglishCenter.Model;
using Microsoft.EntityFrameworkCore;

namespace EnglishCenter.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
