using System.ComponentModel.DataAnnotations;

namespace SrDoc.Api.Models;

public class Document
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Content { get; set; } = string.Empty;

    public DocumentType Type { get; set; } = DocumentType.Direct;

    public DocumentStatus Status { get; set; } = DocumentStatus.Draft;

    [Required]
    public string CreatorId { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public List<Signatory> Signatories { get; set; } = new();
}

public enum DocumentType
{
    Direct = 0,
    Collective = 1
}

public enum DocumentStatus
{
    Draft = 0,
    Sent = 1,
    Completed = 2,
    Canceled = 3
}
