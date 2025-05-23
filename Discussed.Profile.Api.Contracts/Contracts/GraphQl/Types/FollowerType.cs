namespace Discussed.Profile.Api.Contracts.Contracts.GraphQl;

public record FollowerType
{
    public Guid? UserGuid { get; init; }
    public string? Username { get; init; }
    public string? FollowerCount { get; init; }
    public string? FollowingCount { get; init; }
}