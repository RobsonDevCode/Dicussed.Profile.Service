using System.Text.Json.Serialization;

namespace Discussed.Profile.Api.Contracts.Contracts;

public record DeclineRequest
{
    [JsonPropertyName("user_requesting")]
    public required Guid UserRequesting { get; init; }
    
    [JsonPropertyName("user_requested")]
    public required Guid UserRequested { get; init; }
}