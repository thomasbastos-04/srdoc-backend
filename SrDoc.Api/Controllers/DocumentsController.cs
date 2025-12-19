using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SrDoc.Api.Data;
using SrDoc.Api.Models;
using System.Security.Claims;

namespace SrDoc.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly AppDbContext _context;

    public DocumentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDocument([FromBody] Document document)
    {
        if (document.Type == DocumentType.Direct && document.Signatories.Count != 1)
        {
            return BadRequest("Direct documents must have exactly 1 signatory.");
        }

        if (document.Type == DocumentType.Collective && document.Signatories.Count < 3)
        {
            return BadRequest("Collective documents must have at least 3 signatories.");
        }

        document.CreatedAt = DateTime.UtcNow;

        document.CreatorId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "system-user";

        _context.Documents.Add(document);
        await _context.Set<Signatory>().AddRangeAsync(document.Signatories);
        await _context.SaveChangesAsync();

        return Ok(document);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDocument(Guid id)
    {
        var doc = await _context.Documents
            .Include(d => d.Signatories)
            .FirstOrDefaultAsync(d => d.Id == id);

        return doc == null ? NotFound() : Ok(doc);
    }
}