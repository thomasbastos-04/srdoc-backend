using SrDoc.Api.Models;

namespace SrDoc.Api.DTOs.Documents;

public record DocumentUpdateDto(
    string Title,
    string Content,
    DocumentStatus Status
);
