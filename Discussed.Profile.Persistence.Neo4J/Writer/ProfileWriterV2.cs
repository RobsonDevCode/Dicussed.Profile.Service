namespace Discussed.Profile.Persistence.Interfaces.Writer;

public sealed class ProfileWriterV2 : IProfileWriter
{
    public Task<Contracts.Profile> CreateProfile(Contracts.Profile profile, CancellationToken cancellationToken, IEnumerable<string>? filter = null)
    {
        throw new NotImplementedException();
    }
}