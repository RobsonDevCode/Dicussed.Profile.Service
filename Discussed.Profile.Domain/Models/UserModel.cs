using System.Text.Json.Serialization;

namespace Discussed.Profile.Domain.Models;

public record UserModel
{
    [JsonPropertyName("id")]
    public required Guid Id { get; init; }
    
    [JsonPropertyName("user_name")]
    public required string Username { get; init; }
}