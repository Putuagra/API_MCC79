using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class LatihanEmployeeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}