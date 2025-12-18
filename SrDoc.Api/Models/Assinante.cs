namespace SrDoc.Api.Models;

public class Assinante {
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool Assinou { get; set; }
    public DateTime? DataAssinatura { get; set; }
    
    public Guid DocumentoId { get; set; }
}