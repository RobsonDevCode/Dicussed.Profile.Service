namespace Discussed.Profile.Domain.Models;

public class ProfileModelV2
{
    public Guid? Id { get; init ; }
    public string? UserName { get; init ; }
    public string? FollowerCount { get; init ; }
    public string? FollowingCount { get; init ; }
    public string? Bio { get; init ; }
    public bool? Private { get; init ; }
}