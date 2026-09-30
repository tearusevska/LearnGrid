using Domain.Dto;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly IClassService _classService;

    public ClassController(IClassService classService)
    {
        _classService = classService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var classes = await _classService.GetAllAsync();
        return Ok(classes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(Guid id)
    {
        var cls = await _classService.GetByIdAsync(id);
        if (cls == null) return NotFound();
        return Ok(cls);
    }

    [HttpPost]
    public async Task<ActionResult> Insert([FromBody] ClassDto dto)
    {
        var cls = await _classService.InsertAsync(dto);
        return Ok(cls);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] ClassDto dto)
    {
        var cls = await _classService.UpdateAsync(id, dto);
        return Ok(cls);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var cls = await _classService.DeleteAsync(id);
        return Ok(cls);
    }
}