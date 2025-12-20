namespace SrDoc.Api.DTOs;

public record RegisterRequestDTO(string Email, string Password, string FullName);
public record LoginRequestDTO(string Email, string Password);
public record UserResponseDTO(string Id, string Email, string FullName);