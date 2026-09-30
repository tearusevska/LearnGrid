using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using Domain.Models;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EtlSyncLogController : ControllerBase
{
    private readonly IRepository<EtlSyncLog> _repository;

    public EtlSyncLogController(IRepository<EtlSyncLog> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var result = await _repository.GetAllAsync(x => x);
        return Ok(result);
    }
}