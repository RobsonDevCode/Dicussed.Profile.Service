using Discussed.Profile.Persistence.Interfaces.Reader;

namespace Discussed.Profile.Persistence.MySql.Reader;

public class ProfileValidationReader : IProfileValidationReader
{
    public Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UserAlreadyFollowsAsync(Guid userId, Guid userToFollow, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}