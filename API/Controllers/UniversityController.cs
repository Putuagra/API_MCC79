using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


[ApiController]
[Route("api/universities")]
public class UniversityController : GeneralController<University>
{
    private readonly IUniversityRepository _univRepository;
    public UniversityController(IUniversityRepository repository) : base(repository)
    {
        _univRepository = repository;
    }

    [HttpGet("getbyname/{name}")]
    public IActionResult GetByName(string name)
    {
        var university = _univRepository.GetByName(name);
        if (university is null)
        {
            return NotFound();
        }

        return Ok(university);
    }
}