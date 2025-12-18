namespace SrDoc.Api.Models;

public class Signatory {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool HasSigned { get; set; }
    public DateTime? SignedAt { get; set; }
}