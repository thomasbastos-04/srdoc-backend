namespace SrDoc.Api.Models;

public enum DocumentType { Direct, Collective }

public class Document
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DocumentType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Signatory> Signatories { get; set; } = new();
}