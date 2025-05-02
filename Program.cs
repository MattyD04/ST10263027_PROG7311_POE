using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Data;
using ST10263027_PROG7311_POE.Repository;
using ST10263027_PROG7311_POE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register your database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repository with connection string
builder.Services.AddScoped<EmployeeRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new EmployeeRepository(connStr);
});

// Register your services
builder.Services.AddScoped<EmployeeService>();

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
