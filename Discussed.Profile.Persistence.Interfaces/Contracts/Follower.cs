namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public record Follower
{
    public Guid? Id { get; set; }
    public string? Username { get; set; }
    public string? FollowerCount { get; set; }
    public string? FollowingCount { get; set; }


    public bool AllValuesAreNull()
    {
        return Id == null && Username == null && FollowerCount == null && FollowingCount == null;
    }
}