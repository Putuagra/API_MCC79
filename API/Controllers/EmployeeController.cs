using API.DTOs.Employees;
using API.Services;
using API.Utilites.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;

[ApiController]
[Route("api/employees")]
public class EmployeeController : ControllerBase
{
    private readonly EmployeeService _service;

    public EmployeeController(EmployeeService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _service.GetEmployee();

        if (!employees.Any())
        {
            return NotFound(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }
        return Ok(new ResponseHandler<IEnumerable<GetEmployeeDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = employees
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var employee = _service.GetEmployee(guid);
        if (employee is null)
        {
            return NotFound(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }

        return Ok(new ResponseHandler<GetEmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = employee
        });
    }

    [HttpPost]
    public IActionResult Create(NewEmployeeDto newEmployeeDto)
    {
        var CreatedEmployee = _service.CreateEmployee(newEmployeeDto);
        if (CreatedEmployee is null)
        {
            return BadRequest(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }
        return Ok(new ResponseHandler<GetEmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = CreatedEmployee
        });
    }

    [HttpPut]
    public IActionResult Update(GetEmployeeDto getEmployeeDto)
    {
        var update = _service.UpdateEmployee(getEmployeeDto);

        if (update is -1)
        {
            return NotFound(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (update is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }

        return Ok(new ResponseHandler<GetEmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteEmployee(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (delete is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetEmployeeDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }
        return Ok(new ResponseHandler<GetEmployeeDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data deleted"
        });
    }
}
