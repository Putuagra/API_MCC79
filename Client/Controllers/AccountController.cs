using API.DTOs.Accounts;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDto register)
    {

        var result = await _accountRepository.Register(register);
        if (result is null)
        {
            return RedirectToAction("Error", "Home");
        }
        else if (result.Status == "BadRequest")
        {
            ModelState.AddModelError(string.Empty, result.Message);
            TempData["Error"] = $"Something Went Wrong! - {result.Message}!";
            return View();
        }
        else if (result.Status == "OK")
        {
            TempData["Success"] = $"Data has been Successfully Registered! - {result.Message}!";
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var result = await _accountRepository.Login(login);
        if (result is null)
        {
            return RedirectToAction("Error", "Home");
        }
        else if (result.Status == "BadRequest")
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        else if (result.Status == "OK")
        {
            HttpContext.Session.SetString("JWToken", result.Data);
            return RedirectToAction("Index", "Employee");
        }
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Account");
    }
}