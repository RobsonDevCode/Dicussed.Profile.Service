namespace Discussed.Profile.Domain.Models;

public record ProfileModel
{
    public required Guid UserId { get; init; }
    
    public required string FollowerCount { get; init; }
    
    public required string FollowingCount { get; init; }
    
    public required string Bio { get; init; }
    
    public required bool Private { get; init; }
}