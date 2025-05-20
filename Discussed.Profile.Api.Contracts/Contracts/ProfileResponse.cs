using System.Text.Json.Serialization;

namespace Discussed.Profile.Api.Contracts.Contracts;

public record ProfileResponse
{
    [JsonPropertyName("user_id")]
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Follower count is a string as we(rather optimistically want profiles with millions of followers to display as 10 m)
    /// </summary>
    [JsonPropertyName("follower_count")]
    public required string FollowerCount { get; init; }
    
    /// <summary>
    ///  FollowingType count is a string as we(rather optimistically want profiles with millions of following to display as 10 m)
    /// </summary>
    [JsonPropertyName("following_count")]
    public required string FollowingCount { get; init; }
}