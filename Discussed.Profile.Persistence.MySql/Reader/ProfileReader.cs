using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Mapper;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Discussed.Profile.Persistence.MySql.Reader;

public sealed class ProfileReader : IProfileReader
{
    private readonly IMySqlConnectionFactory _mySqlConnectionFactory;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileReader> _logger;
    
    public ProfileReader(IMySqlConnectionFactory mySqlConnectionFactory, IMapper mapper,
        ILogger<ProfileReader> logger)
    {
        _mySqlConnectionFactory = mySqlConnectionFactory;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Interfaces.Contracts.Profile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT * FROM profile WHERE UserId = @userId LIMIT 1";
        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;

        var reader = await command.ExecuteReaderAsync(cancellationToken);
        var result = await _mapper.MapProfileAsync(reader, cancellationToken);
        return result;
    }

    public async Task<bool> UserExistsAsync(Guid userId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(id) FROM profile WHERE UserId = @userId LIMIT 1";
        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        
        await connection.OpenAsync(cancellationToken);
        await using var command = new MySqlCommand(sql, connection);
        
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = userId;
        var count = await command.ExecuteScalarAsync(cancellationToken);

        if (count is null)
        {
            _logger.LogWarning("count returned null when checking user exists.");
            return false;
        }
        
        return (int)count > 0;
    }
}