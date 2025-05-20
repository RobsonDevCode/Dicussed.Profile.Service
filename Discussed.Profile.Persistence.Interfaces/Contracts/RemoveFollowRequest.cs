namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record RemoveFollowRequest
{
    public required Guid UserRequesting { get; init; }
    
    public required Guid UserRequested { get; init; }   
}