using API.DTOs.Universities;
using API.Services;
using API.Utilites.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers;


[ApiController]
[Route("api/universities")]
public class UniversityController : ControllerBase
{
    private readonly UniversityService _service;

    public UniversityController(UniversityService service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var universities = _service.GetUniversity();

        if (!universities.Any())
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }
        return Ok(new ResponseHandler<IEnumerable<GetUniversityDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = universities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var university = _service.GetUniversity(guid);
        if (university is null)
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }

        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = university
        });
    }

    [HttpPost]
    public IActionResult Create(NewUniversityDto newUniversityDto)
    {
        var CreatedUniversity = _service.CreateUniversity(newUniversityDto);
        if (CreatedUniversity is null)
        {
            return BadRequest(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }
        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = CreatedUniversity
        });
    }

    [HttpPut]
    public IActionResult Update(UpdateUniversityDto updateUniversityDto)
    {
        var update = _service.UpdateUniversity(updateUniversityDto);

        if (update is -1)
        {
            return NotFound(new ResponseHandler<UpdateUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (update is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<UpdateUniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }

        return Ok(new ResponseHandler<UpdateUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var delete = _service.DeleteUniversity(guid);

        if (delete is -1)
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        if (delete is 0)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }
        return Ok(new ResponseHandler<GetUniversityDto>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data deleted"
        });
    }

    [HttpGet("by-name/{name}")]
    public IActionResult GetByName(string name)
    {
        var universities = _service.GetUniversity(name);
        if (!universities.Any())
        {
            return NotFound(new ResponseHandler<GetUniversityDto>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No universities found with the given name"
            });
        }

        return Ok(new ResponseHandler<IEnumerable<GetUniversityDto>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Universities found",
            Data = universities
        }); ;
    }
}