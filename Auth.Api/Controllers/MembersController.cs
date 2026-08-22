using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class MembersController : Controller
{
    [Authorize]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}