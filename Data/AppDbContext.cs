using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Models;

namespace ST10263027_PROG7311_POE.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Farmer> Farmers { get; set; }
    }
}
