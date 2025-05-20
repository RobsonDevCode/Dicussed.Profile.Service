using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Writer;
using Microsoft.Extensions.Logging;
using MySqlConnector;

namespace Discussed.Profile.Persistence.MySql.Writer;

public sealed class FollowerWriter : IFollowWriter
{
    private readonly IMySqlConnectionFactory _mySqlConnectionFactory;
    private readonly ILogger<FollowerWriter> _logger;

    public FollowerWriter(IMySqlConnectionFactory mySqlConnectionFactory,
        ILogger<FollowerWriter> logger)
    {
        _mySqlConnectionFactory = mySqlConnectionFactory;
        _logger = logger;
    }

    public async Task AddFollowRequest(Follow request, CancellationToken cancellationToken)
    {
        const string sql = """
                            INSERT INTO followRequests(id, userId, followUser, username)
                            VALUES (@id, @userId, @username, @username);
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

        if (rowsAffected == 0)
        {
            throw new Exception("Query Executed but no rows affected.");
        }

        if (rowsAffected > 1)
        {
            _logger.LogError("Following request have been added but {rows} have been affected! Bug is likely",
                rowsAffected);
        }
    }

    public async Task FollowUser(Follow request, CancellationToken cancellationToken)
    {
        const string sql = """
                           INSERT INTO following(id, userGuid, userFollowing, username)
                           VALUES(@id, @userId, @userFollowing, @username);
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);
        command.Parameters.Add("@id", MySqlDbType.Guid).Value = request.Id;
        command.Parameters.Add("@userId", MySqlDbType.Guid).Value = request.UserId;
        command.Parameters.Add("@userFollowing", MySqlDbType.Guid).Value = request.FollowerUserId;
        command.Parameters.Add("@username", MySqlDbType.VarChar).Value = request.Username;

        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

        switch (rowsAffected)
        {
            case 0:
                throw new Exception("Query Executed but no rows affected.");
            case > 1:
                _logger.LogError("Following request have been added but {rows} have been affected! Bug is likely",
                    rowsAffected);
                break;
        }
    }

    public async Task RemoveFollowRequest(RemoveFollowRequest request, CancellationToken cancellationToken)
    {
        const string sql = """
                            DELETE FROM followRequests WHERE userId= @userRequesting AND followUser= @userRequested;
                           """;

        await using var connection = _mySqlConnectionFactory.CreateUserInfoConnection();
        await connection.OpenAsync(cancellationToken);

        await using var command = new MySqlCommand(sql, connection);

        command.Parameters.Add("@userRequesting", MySqlDbType.Guid).Value = request.UserRequesting;
        command.Parameters.Add("@userRequested", MySqlDbType.Guid).Value = request.UserRequested;

        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);

        switch (rowsAffected)
        {
            case 0:
                throw new Exception("Query Executed but no rows affected.");
            case > 1:
                _logger.LogError("Follow request has been removed but {rows} have been affected! Bug is likely",
                    rowsAffected);
                break;
        }
    }
}