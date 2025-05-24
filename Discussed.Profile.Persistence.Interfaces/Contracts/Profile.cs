namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record Profile
{
    public required Guid UserId { get; init; }
    public required string Username { get; init ; }
    public required string Bio { get; init ; }
    public required int FollowerCount { get; init ; }
    public required int FollowingCount { get; init ; }
    public required bool Private { get; init ; }
}