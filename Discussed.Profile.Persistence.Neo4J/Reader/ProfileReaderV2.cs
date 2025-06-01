using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Mapper;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Neo4j.Driver;

namespace Discussed.Profile.Persistence.Neo4J.Reader;

public class ProfileReaderV2 : IProfileReader
{
    private readonly INeo4JConnectionFactory _connectionFactory;
    private readonly IMapper _mapper;

    public ProfileReaderV2(INeo4JConnectionFactory connectionFactory, IMapper mapper)
    {
        _connectionFactory = connectionFactory;
        _mapper = mapper;
    }

    public Task<Interfaces.Contracts.Profile?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<ProfileV2> GetByIdAsync(Guid userId, IEnumerable<string> fields,
        CancellationToken cancellationToken)
    {
        await using var session = _connectionFactory.CreateAsyncSession();

        var result = await session.ExecuteReadAsync(async tx =>
        {
            var cursor = await tx.RunAsync("""
                                            MATCH (p:Profile {id: $id})
                                            OPTIONAL MATCH ()-[:FOLLOWS]->(p)
                                            RETURN 
                                                p.id as id,
                                                p.username as username,
                                                p.bio as bio,
                                                p.isPrivate as isPrivate,
                                                count(*) as followerCount
                                           """,
                userId);

            return await cursor.SingleAsync<ProfileV2>(result => new ProfileV2
            {
                UserId = result["id"].As<Guid>(),
                Username = result["username"].As<string>(),
                Bio = result["bio"].As<string>(),
                Private = result["isPrivate"].As<bool>(),
                FollowerCount = result["followerCount"].As<int>()
            });
        });

        return result;
    }
}