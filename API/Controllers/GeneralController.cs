namespace API.Controllers;

/*public class GeneralController<TRepository, TEntity> : ControllerBase
    where TEntity : class
    where TRepository : IGeneralRepository<TEntity>
{
    protected readonly TRepository _repository;

    public GeneralController(TRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var entities = _repository.GetAll();
        if (!entities.Any())
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }
        return Ok(new ResponseHandler<IEnumerable<TEntity>>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entities
        });
    }

    [HttpGet("{guid}")]
    public IActionResult GetByGuid(Guid guid)
    {
        var entity = _repository.GetByGuid(guid);
        if (entity is null)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "No data found in this table"
            });
        }

        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data found",
            Data = entity
        });

    }

    [HttpPost]
    public IActionResult Create(TEntity entity)
    {
        var isCreated = _repository.Create(entity);
        if (isCreated is false)
        {
            return BadRequest(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status400BadRequest,
                Status = HttpStatusCode.BadRequest.ToString(),
                Message = "Data not created"
            });
        }
        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data created",
            Data = entity
        });
    }

    [HttpPut]
    public IActionResult Update(TEntity entity)
    {
        // Reflection
        var getGuid = (Guid)typeof(TEntity).GetProperty("Guid")!.GetValue(entity)!;
        var isFound = _repository.IsExist(getGuid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isUpdated = _repository.Update(entity);
        if (!isUpdated)
        {
            return BadRequest(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check your data"
            });
        }

        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data updated"
        });
    }

    [HttpDelete]
    public IActionResult Delete(Guid guid)
    {
        var isFound = _repository.IsExist(guid);

        if (isFound is false)
        {
            return NotFound(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status404NotFound,
                Status = HttpStatusCode.NotFound.ToString(),
                Message = "Id not found"
            });
        }

        var isDeleted = _repository.Delete(guid);
        if (!isDeleted)
        {
            return BadRequest(new ResponseHandler<TEntity>
            {
                Code = StatusCodes.Status500InternalServerError,
                Status = HttpStatusCode.InternalServerError.ToString(),
                Message = "Check connection to database"
            });
        }
        return Ok(new ResponseHandler<TEntity>
        {
            Code = StatusCodes.Status200OK,
            Status = HttpStatusCode.OK.ToString(),
            Message = "Data deleted"
        });
    }
}*/
