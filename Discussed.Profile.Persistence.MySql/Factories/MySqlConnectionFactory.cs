using Discussed.Profile.Persistence.Interfaces.Factories;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
namespace Discussed.Profile.Persistence.MySql.Factories;

public sealed class MySqlConnectionFactory : IMySqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public MySqlConnectionFactory (IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public MySqlConnection CreateUserInfoConnection()
    {
        var connectionString = _configuration.GetConnectionString("UserInfoDb");
        return new MySqlConnection(connectionString);
    }
}