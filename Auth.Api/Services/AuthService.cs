using System.Security.Claims;
using Auth.Api.Data;
using Microsoft.AspNetCore.Identity;

namespace Auth.Api.Services;

public class AuthService(
    UserManager<IdentityUser> userManager,
    RoleManager<IdentityRole> roleManager,
    SignInManager<IdentityUser> signInManager,
    IHttpContextAccessor httpContextAccessor) : IAuthService
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

    public async Task ToggleReportsAuthorization()
    {
        var user = await GetUser();
        var hasClaim = await CanViewReports();

        if (!hasClaim)
        {
            await userManager.AddClaimAsync(user, new Claim(RoleSeeder.CLAIM_TYPE, RoleSeeder.CLAIM_VALUE));
        }
        else
        {
            await userManager.RemoveClaimAsync(user, new Claim(RoleSeeder.CLAIM_TYPE, RoleSeeder.CLAIM_VALUE));
        }

        await signInManager.RefreshSignInAsync(user);
    }

    public async Task<bool> CanViewReports()
    {
        var user = await GetUser();
        var claims = await userManager.GetClaimsAsync(user);
        var hasClaim = claims.Any(c => c.Type == RoleSeeder.CLAIM_TYPE && c.Value == RoleSeeder.CLAIM_VALUE);
        return hasClaim;
    }

    private async Task<IdentityUser> GetUser()
    {
        var principal = httpContextAccessor.HttpContext?.User;
        if (principal == null) return null;

        var user = await userManager.GetUserAsync(principal);
        return user;
    }
}