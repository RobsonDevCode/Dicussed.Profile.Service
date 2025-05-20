namespace Discussed.Profile.Persistence.Interfaces.Reader;

public interface IProfileReader
{
    Task<Contracts.Profile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
}