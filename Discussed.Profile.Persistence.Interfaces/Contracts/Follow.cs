namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record Follow
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required Guid UserId { get; init; }
    public required Guid FollowerUserId { get; init; }
    public required string Username { get; init; }
}