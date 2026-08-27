using Auth.Api.Models;
using Auth.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class MembersController(IAuthService authService) : Controller
{
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var isAuth = HttpContext.User.Identity?.IsAuthenticated;
        var name = HttpContext.User.Identity?.Name;
        var claims = HttpContext.User.Claims.ToList();

        ViewBag.IsAuth = isAuth;
        ViewBag.Name = name;
        ViewBag.Claims = claims;

        var model = new MemberViewModel
        {
            CanViewReports = await authService.CanViewReports(),
        };

        return View(model);
    }

    public async Task<IActionResult> ToggleReports()
    {
        await authService.ToggleReportsAuthorization();
        return RedirectToAction(nameof(Index));
    }

    [Authorize(Policy = "CanViewReports")]
    public IActionResult Reports()
    {
        return View();
    }

    // Essentially means:"You haven't successfully authenticated for this resource."
    public async Task<IActionResult> ChallengeTest()
    {
        return Challenge();
    }

    // Means: "I know who you are, but you're not allowed to do this."
    public async Task<IActionResult> ForbidTest()
    {
        return Forbid();
    }
}