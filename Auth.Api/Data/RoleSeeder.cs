// using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Data;

public static class RoleSeeder
{
    public const string ROLE_ADMIN = "Admin";
    public const string USER_EMAIL = "user@example.com";
    public const string CLAIM_TYPE = "Permission";
    public const string CLAIM_VALUE = "CanViewReports";

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
            // var claims = await userManager.GetClaimsAsync(user);
            // var hasClaim = claims.Any(c => c.Type == CLAIM_TYPE && c.Value == CLAIM_VALUE);
            // // await userManager.RemoveClaimAsync(user, new Claim(CLAIM_TYPE, CLAIM_VALUE));

            // if (!hasClaim)
            // {
            //     await userManager.AddClaimAsync(user, new Claim(CLAIM_TYPE, CLAIM_VALUE));
            // }

            // var POLICY_TYPE = "Department";
            // var POLICY_VALUE = "Finance";
            // var hasDepartment = claims.Any(d => d.Type == POLICY_TYPE && d.Value == POLICY_VALUE);
            // if (!hasDepartment)
            // {
            //     await userManager.AddClaimAsync(user, new Claim(POLICY_VALUE, POLICY_VALUE));
            // }
        }
    }

    public static async Task ClearUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var users = userManager.Users.ToList();

        foreach (var user in users)
        {
            await userManager.DeleteAsync(user);
        }
    }
}