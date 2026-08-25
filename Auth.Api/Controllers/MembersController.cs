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
        
        return View();
    }
}