using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Domain.Models;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecommendedResourceController : ControllerBase
{
    private readonly IRepository<RecommendedResource> _repository;

    public RecommendedResourceController(IRepository<RecommendedResource> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync(x => x);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var result = await _repository.Get(x => x, predicate: x => x.Id == id);
        if (result == null) return NotFound();
        return Ok(result);
    }
}