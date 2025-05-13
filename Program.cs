using Microsoft.EntityFrameworkCore;
using ST10263027_PROG7311_POE.Data;
using ST10263027_PROG7311_POE.Repository;
using ST10263027_PROG7311_POE.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registering repositories with connection strings
// Registering employee repository
builder.Services.AddScoped<EmployeeRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new EmployeeRepository(connStr);
});

// Registering farmer repository
builder.Services.AddScoped<FarmerRepository>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("DefaultConnection");
    return new FarmerRepository(connStr);
});

//Registering Employee service
builder.Services.AddScoped<EmployeeService>();
//Registering Farmer service
builder.Services.AddScoped<FarmerService>();

// Configure session state
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

;
app.UseSession();

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
//***********************************************End of file*****************************************//

