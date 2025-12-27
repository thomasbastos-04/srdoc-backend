using SrDoc.Api.Models;

namespace SrDoc.Api.DTOs.Documents;

public record DocumentCreateDto(
    string Title,
    string Content,
    DocumentType Type,
    List<SignatoryCreateDto> Signatories
);

public record SignatoryCreateDto(
    string Name,
    string Email
);
