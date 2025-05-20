namespace Discussed.Profile.Domain.Models;

public record FollowRequestModel
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required Guid UserToFollow { get; init; }
    public required bool IsPrivate { get; init; }
    
}