using Discussed.Profile.Persistence.Interfaces.Writer;

namespace Discussed.Profile.Persistence.MySql.Writer;

public sealed class ProfileWriter : IProfileWriter
{
    public Task<Interfaces.Contracts.Profile> CreateProfile(Interfaces.Contracts.Profile profile,
        CancellationToken cancellationToken, IEnumerable<string>? filter = null)
    {
        throw new NotImplementedException();
    }
}