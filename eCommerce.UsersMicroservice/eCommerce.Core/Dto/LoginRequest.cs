namespace eCommerce.Core.Dto;

public record LoginRequest(
    string? Email,
    string? Password);