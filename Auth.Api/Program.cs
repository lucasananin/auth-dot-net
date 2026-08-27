using Auth.Api.Data;
using Auth.Api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllersWithViews();
services.AddDbContext<AppDbContext>(options => { options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")); });
services.AddDefaultIdentity<IdentityUser>()
        .AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<AppDbContext>();
services.AddScoped<IAuthService, AuthService>();
services.AddAuthorizationBuilder()
        .AddPolicy("CanViewReports", policy =>
        {
            policy.RequireClaim("Permission", "CanViewReports");
            // policy.RequireClaim("Department", "Finance");
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var serviceProvicer = scope.ServiceProvider;
    await RoleSeeder.SeedRolesAsync(serviceProvicer);
    await RoleSeeder.SeedUserDataAsync(serviceProvicer);
    // await RoleSeeder.ClearUsers(serviceProvicer);
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
app.MapRazorPages();

app.Run();