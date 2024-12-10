namespace eCommerce.Core.Dto;

public record AuthenticationResponse(
    Guid UserId,
    string? Email,
    string? PersonName,
    string? Gender,
    string? Token,
    bool Success)
{
    // Parameterless constructor for serialization
    public AuthenticationResponse() : this(default, default, default, default, default, default) { }
}