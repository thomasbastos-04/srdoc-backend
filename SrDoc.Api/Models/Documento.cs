namespace SrDoc.Api.Models;

public enum TipoDocumento { Direto, Coletivo }

public class Documento {
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public TipoDocumento Tipo { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    
    // Relacionamento
    public string CriadorId { get; set; } = string.Empty;
    public List<Assinante> Assinantes { get; set; } = new();
}