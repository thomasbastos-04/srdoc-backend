using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrDoc.Api.Data;
using SrDoc.Api.DTOs.Documents;
using SrDoc.Api.Models;
using System.Security.Claims;

namespace SrDoc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DocumentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DocumentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DocumentCreateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        if (dto.Type == DocumentType.Direct && (dto.Signatories == null || dto.Signatories.Count != 1))
            return BadRequest("Direct documents must have exactly 1 signatory.");

        if (dto.Type == DocumentType.Collective && (dto.Signatories == null || dto.Signatories.Count < 3))
            return BadRequest("Collective documents must have at least 3 signatories.");

        var document = new Document
        {
            Title = dto.Title?.Trim() ?? string.Empty,
            Content = dto.Content ?? string.Empty,
            Type = dto.Type,
            Status = DocumentStatus.Draft,
            CreatorId = userId,
            CreatedAt = DateTime.UtcNow,
            Signatories = dto.Signatories.Select(s => new Signatory
            {
                Name = s.Name?.Trim() ?? string.Empty,
                Email = s.Email?.Trim() ?? string.Empty
            }).ToList()
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = document.Id }, ToResponse(document));
    }

    [HttpGet]
    public async Task<IActionResult> GetMyDocuments()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var docs = await _context.Documents
            .AsNoTracking()
            .Include(d => d.Signatories)
            .Where(d => d.CreatorId == userId)
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();

        return Ok(docs.Select(ToResponse));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var doc = await _context.Documents
            .Include(d => d.Signatories)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doc == null) return NotFound();
        if (doc.CreatorId != userId) return Forbid();

        return Ok(ToResponse(doc));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DocumentUpdateDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var doc = await _context.Documents
            .Include(d => d.Signatories)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doc == null) return NotFound();
        if (doc.CreatorId != userId) return Forbid();

        if (doc.Status == DocumentStatus.Completed)
            return BadRequest("Completed documents cannot be edited.");

        doc.Title = dto.Title?.Trim() ?? doc.Title;
        doc.Content = dto.Content ?? doc.Content;

        var previousStatus = doc.Status;
        doc.Status = dto.Status;

        doc.UpdatedAt = DateTime.UtcNow;

        if (previousStatus != DocumentStatus.Completed && doc.Status == DocumentStatus.Completed)
            doc.CompletedAt = DateTime.UtcNow;

        if (doc.Status != DocumentStatus.Completed)
            doc.CompletedAt = null;

        await _context.SaveChangesAsync();

        return Ok(ToResponse(doc));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId)) return Unauthorized();

        var doc = await _context.Documents
            .Include(d => d.Signatories)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (doc == null) return NotFound();
        if (doc.CreatorId != userId) return Forbid();

        _context.RemoveRange(doc.Signatories);
        _context.Documents.Remove(doc);

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static DocumentResponseDto ToResponse(Document d)
    {
        return new DocumentResponseDto(
            d.Id,
            d.Title,
            d.Content,
            d.Type,
            d.Status,
            d.CreatorId,
            d.CreatedAt,
            d.UpdatedAt,
            d.CompletedAt,
            d.Signatories.Select(s => new SignatoryResponseDto(s.Id, s.Name, s.Email)).ToList()
        );
    }
}
