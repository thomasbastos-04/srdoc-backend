using SrDoc.Api.Models;

namespace SrDoc.Api.DTOs.Documents;

public record DocumentResponseDto(
    Guid Id,
    string Title,
    string Content,
    DocumentType Type,
    DocumentStatus Status,
    string CreatorId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    DateTime? CompletedAt,
    List<SignatoryResponseDto> Signatories
);

public record SignatoryResponseDto(
    Guid Id,
    string Name,
    string Email
);
