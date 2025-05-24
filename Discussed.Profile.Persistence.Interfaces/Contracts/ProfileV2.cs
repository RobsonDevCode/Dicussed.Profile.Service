namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record ProfileV2
{
    public Guid? UserId { get; set; }
    public string? Username { get; set; }
    public string? Bio { get; set; }
    public int? FollowerCount { get; set; }
    public int? FollowingCount { get; set; }
    public bool? Private { get; set; }
}