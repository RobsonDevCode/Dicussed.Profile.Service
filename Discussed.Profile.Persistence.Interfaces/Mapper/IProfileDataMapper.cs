using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Mapper;

public interface IProfileDataMapper
{
    Task<Contracts.Profile?> MapProfileAsync(MySqlDataReader reader, CancellationToken cancellationToken); 
}