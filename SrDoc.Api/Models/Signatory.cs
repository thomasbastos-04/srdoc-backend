using System.ComponentModel.DataAnnotations;

namespace SrDoc.Api.Models;

public class Signatory
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(180)]
    public string Email { get; set; } = string.Empty;

    public Guid DocumentId { get; set; }
    public Document Document { get; set; } = default!;
}
