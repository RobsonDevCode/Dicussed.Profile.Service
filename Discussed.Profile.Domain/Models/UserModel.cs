
namespace Discussed.Profile.Domain.Models;

public record UserModel
{
    public Guid? Id { get; init; }
    public string? Username { get; init; }
    
    public string? Email { get; init; }
    
    public string? Number { get; init; }
    
    public DateTime? CreatedAt { get; init; }
    
    public DateTime? UpdatedAt { get; init; }
    
    public bool? EmailConfirmed { get; init; }
}