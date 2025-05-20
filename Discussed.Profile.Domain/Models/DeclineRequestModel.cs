namespace Discussed.Profile.Domain.Models;

public record DeclineRequestModel
{
    public required Guid UserRequesting { get; init; }
    
    public required Guid UserRequested { get; init; }
}