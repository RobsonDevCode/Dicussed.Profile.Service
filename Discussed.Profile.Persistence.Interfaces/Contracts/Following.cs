namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record Following
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public required Guid FollowingId { get; init; }
}