using API.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class GeneralController<TEntity> : ControllerBase
    where TEntity : class
{
    protected readonly IGeneralRepository<TEntity> _repository;

    public GeneralController(IGeneralRepository<TEntity> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _repository.GetAll();
        if (!entities.Any())
        {
            return NotFound();
        }
        return Ok(entities);
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _repository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound();
        }

        return Ok(entity);

    }

    [HttpPost]
    public IActionResult Create(TEntity entity)
    {
        var isCreated = _repository.Create(entity);
        return Ok(isCreated);
    }

    [HttpPut]
    public IActionResult Update(TEntity entity)
    {
        var isUpdated = _repository.Update(entity);
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
