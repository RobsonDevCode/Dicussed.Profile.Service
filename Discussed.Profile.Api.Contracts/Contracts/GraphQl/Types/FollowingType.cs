namespace Discussed.Profile.Api.Contracts.Contracts.GraphQl;

public record FollowingType
{
    public required Guid UserId { get; init; }

    public required string Username { get; init; }

    public required Guid FollowingId { get; init; }
}