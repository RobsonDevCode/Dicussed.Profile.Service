namespace Discussed.Profile.Persistence.Interfaces.Reader;

public interface IProfileValidationReader
{
    Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken);
    Task<bool> UserAlreadyFollowsAsync(Guid userId,Guid userToFollow ,CancellationToken cancellationToken);
}