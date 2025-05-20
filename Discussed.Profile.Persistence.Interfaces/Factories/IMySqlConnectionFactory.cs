using MySqlConnector;

namespace Discussed.Profile.Persistence.Interfaces.Factories;

public interface IMySqlConnectionFactory
{
    MySqlConnection CreateUserInfoConnection();
}