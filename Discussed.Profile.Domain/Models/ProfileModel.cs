namespace Discussed.Profile.Domain.Models;

public record ProfileModel
{
    public required Guid UserId { get; init; }
    
    public required string FollowerCount { get; init; }
    
    public required string FollowingCount { get; init; }
}