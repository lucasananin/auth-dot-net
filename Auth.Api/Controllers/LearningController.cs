using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class LearningController : Controller
{
    public async Task<IActionResult> Index()
    {
        return View();
    }
}