using API.Contracts;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/roles")]
public class RoleController : ControllerBase
{
    private readonly IRoleRepository _repository;
    public RoleController(IRoleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var roles = _repository.GetAll();

        if (!roles.Any())
        {
            return NotFound();
        }

        return Ok(roles);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid id)
    {
        var role = _repository.GetByGuid(id);

        if (role is null)
        {
            return NotFound();
        }
        return Ok(role);
    }

    [HttpPost]
    public IActionResult Create(Role role)
    {
        var created = _repository.Create(role);
        return Ok(created);
    }

    [HttpPut]
    public IActionResult Update(Role role)
    {
        var isUpdated = _repository.Update(role);
        if (!isUpdated)
        {
            return NotFound();
        }
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isDeleted = _repository.Delete(guid);
        if (!isDeleted)
        {
            return NotFound();
        }
        return Ok();
    }
}
