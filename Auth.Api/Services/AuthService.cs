using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<bool> RoleExistsAsync(string role)
    {
        return await roleManager.RoleExistsAsync("Admin");
    }

    public async Task CreateRoleAsync(string role)
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    public async Task AssignRoleToUser(string role)
    {
        var user = await userManager.FindByEmailAsync("user@example.com");

        if (user != null)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}