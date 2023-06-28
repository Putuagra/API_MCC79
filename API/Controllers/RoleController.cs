using API.DTOs.Roles;
using API.Services;
using API.Utilites.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;
[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly RoleService _service;

    public RoleController(RoleService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var roles = _service.GetRole();

        if (!roles.Any())
        {
            return NotFound(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }
        return Ok(new ResponseHandler<IEnumerable<GetRoleDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = roles
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var role = _service.GetRole(guid);
        if (role is null)
        {
            return NotFound(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }

        return Ok(new ResponseHandler<GetRoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = role
        });
    }

    [HttpPost]
    public IActionResult Create(NewRoleDto newRoleDto)
    {
        var CreatedRole = _service.CreateRole(newRoleDto);
        if (CreatedRole is null)
        {
            return BadRequest(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }
        return Ok(new ResponseHandler<GetRoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = CreatedRole
        });
    }

    [HttpPut]
    public IActionResult Update(GetRoleDto getRoleDto)
    {
        var update = _service.UpdateRole(getRoleDto);

        if (update is -1)
        {
            return NotFound(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (update is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }

        return Ok(new ResponseHandler<GetRoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteRole(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (delete is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetRoleDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }
        return Ok(new ResponseHandler<GetRoleDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data deleted"
        });
    }
}
