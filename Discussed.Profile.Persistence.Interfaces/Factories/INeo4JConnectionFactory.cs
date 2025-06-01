using Neo4j.Driver;

namespace Discussed.Profile.Persistence.Interfaces.Factories;

public interface INeo4JConnectionFactory
{
    IAsyncSession CreateAsyncSession();
}