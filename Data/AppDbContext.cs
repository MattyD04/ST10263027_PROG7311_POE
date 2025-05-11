using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Models;

namespace ST10263027_PROG7311_POE.Data
{
    //Database context class for Entity Framework Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } //Constructor to pass options to the base class
        public DbSet<Employee> Employees { get; set; } //DbSet for Employee entity
        public DbSet<Farmer> Farmers { get; set; } //DbSet for Farmer entity
        public DbSet<Product> Products { get; set; } //DbSet for Product entity
    }
}
//***********************************************End of file*****************************************//
