using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EsApi.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public AttachmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Attachment>>> Get()
    {
        return await _context.Attachments.ToListAsync();
    }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<List<Attachment>>> GetByEntity(
        string entityType,
        int entityId)
    {
        return await _context.Attachments
            .Where(x =>
                x.EntityType == entityType &&
                x.EntityId == entityId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Attachment>> Post(
        Attachment attachment)
    {
        _context.Attachments.Add(attachment);

        await _context.SaveChangesAsync();

        return Ok(attachment);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(x => x.Idattachment == id);

        if (attachment == null)
            return NotFound();

        _context.Attachments.Remove(attachment);

        await _context.SaveChangesAsync();

        return NoContent();
    }
}