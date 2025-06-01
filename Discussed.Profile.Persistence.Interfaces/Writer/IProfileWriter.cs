namespace Discussed.Profile.Persistence.Interfaces.Writer;

public interface IProfileWriter
{
    Task<Contracts.Profile> CreateProfile(Contracts.Profile profile, CancellationToken cancellationToken,
        IEnumerable<string>? filter = null);
}