using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Data;
using ST10263027_PROG7311_POE.Services;
using ST10263027_PROG7311_POE.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    {
    }
}
