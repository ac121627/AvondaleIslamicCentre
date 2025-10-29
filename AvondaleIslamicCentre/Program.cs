using AvondaleIslamicCentre.Areas.Identity.Data;
using AvondaleIslamicCentre;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("AvondaleIslamicCentreDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AvondaleIslamicCentreDbContextConnection' not found.");

builder.Services.AddDbContext<AICDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AICUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AICDbContext>();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Initialize DB (roles, users, seed data)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await AICDbInitializer.InitializeAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database seeding error: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
