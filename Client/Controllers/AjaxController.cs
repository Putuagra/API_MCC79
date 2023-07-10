using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class AjaxController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}