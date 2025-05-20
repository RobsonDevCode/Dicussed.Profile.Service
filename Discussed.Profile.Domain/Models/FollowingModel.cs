namespace Discussed.Profile.Domain.Models;

public class FollowingModel
{
    /// <summary>
    /// User following is retrieved for
    /// </summary>
    public required Guid UserId { get; init; }
    
    /// <summary>
    /// Username: the username of the followed user
    /// </summary>
    public required string Username { get; init; }
    
    /// <summary>
    /// Id of the user followed
    /// </summary>
    public required Guid FollowingId { get; init; }
}