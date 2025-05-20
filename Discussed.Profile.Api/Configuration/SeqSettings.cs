namespace Discussed.Profile.Api.Configuration;

public record SeqSettings
{
    public required string Uri  { get; init; }
    
    public required string Headers { get; init; }
}