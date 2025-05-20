using System.Text.Json.Serialization;

namespace Discussed.Profile.Api.Contracts.Contracts;

public record FollowingResponse
{
    [JsonPropertyName("user_id")]
    public required Guid UserId { get; init; }
    
    [JsonPropertyName("user_name")]
    public required string Username { get; init; }
    
    [JsonPropertyName("following_id")]
    public required Guid FollowingId { get; init; }
}