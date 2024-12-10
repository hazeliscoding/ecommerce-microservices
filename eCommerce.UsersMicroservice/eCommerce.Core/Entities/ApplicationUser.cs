namespace eCommerce.Core.Entities;

/// <summary>
/// Define the ApplicationUser class which acts as an entity model class to store user details in the database
/// </summary>
public class ApplicationUser
{
    public Guid UserId { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? PersonName { get; set; }
    public string? Gender { get; set; }
}