using API.DTOs.Employees;
using API.Utilities.Enums;
using Client.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

public class EmployeeController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [Authorize(Roles = $"{nameof(RoleLevel.Admin)}")]
    public async Task<IActionResult> Index()
    {
        var result = await _employeeRepository.Get();
        var ListEmployee = new List<GetEmployeeDto>();

        if (result.Data != null)
        {
            ListEmployee = result.Data.ToList();
        }
        return View(ListEmployee);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GetEmployeeDto getEmployeeDto)
    {
        var result = await _employeeRepository.Post(getEmployeeDto);
        if (result.Status == "200")
        {
            TempData["Success"] = "Data berhasil masuk";
            return RedirectToAction(nameof(Index));
        }
        else if (result.Status == "409")
        {
            ModelState.AddModelError(string.Empty, result.Message);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid guid)
    {
        var result = await _employeeRepository.Get(guid);

        if (result.Data?.Guid is null)
        {
            return RedirectToAction(nameof(Index));
        }
        var employee = new GetEmployeeDto()
        {
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName,
            Email = result.Data.Email,
            Gender = result.Data.Gender,
            BirthDate = result.Data.BirthDate,
            HiringDate = result.Data.HiringDate,
            PhoneNumber = result.Data.PhoneNumber,
            Guid = result.Data.Guid,
            Nik = result.Data.Nik
        };

        return View(employee);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(GetEmployeeDto getEmployeeDto)
    {
        if (!ModelState.IsValid)
        {
            return View(getEmployeeDto);
        }
        var result = await _employeeRepository.Put(getEmployeeDto.Guid, getEmployeeDto);
        if (result.Status == "200")
        {
            TempData["Success"] = "Data berhasil";
        }
        else if (result.Status == "409")
        {
            TempData["Fail"] = "Error";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid guid)
    {
        var result = await _employeeRepository.Delete(guid);
        if (result.Status == "200")
        {
            return RedirectToAction(nameof(Index));
        }
        return RedirectToAction(nameof(Index));
    }
}