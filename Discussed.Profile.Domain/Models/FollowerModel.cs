namespace Discussed.Profile.Domain.Models;

public record FollowerModel
{
    public Guid? Id { get; init; }
    public string? Username { get; init; }
    public string? FollowerCount { get; init; }
    public string? FollowingCount { get; init; }
}