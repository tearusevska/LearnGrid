using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EtlController : ControllerBase
{
    private readonly IResourceEtlService _etlService;

    public EtlController(IResourceEtlService etlService)
    {
        _etlService = etlService;
    }

    [HttpPost("sync")]
    public async Task<ActionResult> Sync()
    {
        await _etlService.SyncRecommendedResourcesAsync();
        return Ok("ETL sync завршен.");
    }
}