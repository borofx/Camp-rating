using Camp_rating.Data;
using Microsoft.EntityFrameworkCore;
using Camp_rating.Models;
using Microsoft.AspNetCore.Identity;
using NuGet.Common;
//konfigurirane na dbcontext

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//configuring na identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
// инициализиране на админски потребител
async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
{
    using var scope = serviceProvider.CreateScope();
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Създаване на роли, ако не съществуват
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        var roleResult = await roleManager.CreateAsync(new IdentityRole("Admin"));
        if (!roleResult.Succeeded)
        {
            throw new Exception("Failed to create Admin role");
        }
    }

    if (!await roleManager.RoleExistsAsync("User"))
    {
        var roleResult = await roleManager.CreateAsync(new IdentityRole("User"));
        if (!roleResult.Succeeded)
        {
            throw new Exception("Failed to create User role");
        }
    }

    // Проверка за съществуващ админ
    var adminEmail = "admin@camp.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);

    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Admin",
            LastName = "User"
        };

        var createResult = await userManager.CreateAsync(adminUser, "Admin123!");
        if (!createResult.Succeeded)
        {
            var errors = string.Join(", ", createResult.Errors.Select(e => e.Description));
            throw new Exception($"Failed to create admin user: {errors}");
        }

        var addRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
        if (!addRoleResult.Succeeded)
        {
            var errors = string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
            throw new Exception($"Failed to add admin role: {errors}");
        }
    }
}
await SeedAdminUserAsync(app.Services);
app.Run();
