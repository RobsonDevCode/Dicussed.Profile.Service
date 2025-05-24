namespace Discussed.Profile.Api.Contracts.Contracts.GraphQl.Types.Profile;

public record ProfileType
{
    public Guid? UserId { get; init; }
    public string? FollowerCount { get; init; }
    public string? FollowingCount { get; init; }  
    public bool? Private { get; init; }
    public string? Bio { get; init; }
}