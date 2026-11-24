using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace EsApi.Controllers.v1;

[ApiController]
[Authorize(Roles = LoginUser.AdministratorRole)]
[Route("api/v1/[controller]")]
public class AttachmentsController : ControllerBase
{
    private readonly AppDbContext _context;
    private static readonly Expression<Func<Attachment, AttachmentResponse>> ToResponse = attachment =>
        new AttachmentResponse(attachment.Idattachment, attachment.EntityType, attachment.EntityId,
            attachment.FileName, attachment.UploadDate);

    public AttachmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<AttachmentResponse>>> Get(CancellationToken cancellationToken)
    {
        return await _context.Attachments.AsNoTracking().Select(ToResponse).ToListAsync(cancellationToken);
    }

    [HttpGet("{entityType}/{entityId}")]
    public async Task<ActionResult<List<AttachmentResponse>>> GetByEntity(
        string entityType,
        int entityId,
        CancellationToken cancellationToken)
    {
        return await _context.Attachments.AsNoTracking()
            .Where(x =>
                x.EntityType == entityType &&
                x.EntityId == entityId)
            .Select(ToResponse)
            .ToListAsync(cancellationToken);
    }

    [HttpPost]
    public async Task<ActionResult<AttachmentResponse>> Post(
        CreateAttachmentRequest request,
        CancellationToken cancellationToken)
    {
        var attachment = new Attachment
        {
            EntityType = request.EntityType,
            EntityId = request.EntityId,
            FileName = request.FileName,
            FilePath = request.FilePath,
            UploadDate = DateTime.UtcNow
        };
        _context.Attachments.Add(attachment);

        await _context.SaveChangesAsync(cancellationToken);

        return Ok(new AttachmentResponse(attachment.Idattachment, attachment.EntityType, attachment.EntityId,
            attachment.FileName, attachment.UploadDate));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var attachment = await _context.Attachments
            .FirstOrDefaultAsync(x => x.Idattachment == id, cancellationToken);

        if (attachment == null)
            return NotFound();

        _context.Attachments.Remove(attachment);

        await _context.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    public sealed record CreateAttachmentRequest(
        [Required, StringLength(20)] string EntityType,
        [Range(1, int.MaxValue)] int EntityId,
        [Required, StringLength(255)] string FileName,
        [Required, StringLength(500)] string FilePath);

    public sealed record AttachmentResponse(
        int Idattachment, string EntityType, int EntityId, string FileName, DateTime UploadDate);
}
