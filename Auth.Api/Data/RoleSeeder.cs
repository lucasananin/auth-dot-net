using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Data;

public static class RoleSeeder
{
    const string ROLE_ADMIN = "Admin";
    const string USER_EMAIL = "user@example.com";
    const string CLAIM_TYPE = "Permission";
    const string CLAIM_VALUE = "CanViewReports";

    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        string[] roles = [ROLE_ADMIN, "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public static async Task SeedUserDataAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var user = await userManager.FindByEmailAsync(USER_EMAIL);

        if (user != null)
        {
            if (!await userManager.IsInRoleAsync(user, ROLE_ADMIN))
            {
                await userManager.AddToRoleAsync(user, ROLE_ADMIN);
            }

            var claims = await userManager.GetClaimsAsync(user);
            var hasClaim = claims.Any(c => c.Type == CLAIM_TYPE && c.Value == CLAIM_VALUE);

            if (!hasClaim)
            {
                await userManager.AddClaimAsync(user, new Claim(CLAIM_TYPE, CLAIM_VALUE));
            }
        }
    }
}