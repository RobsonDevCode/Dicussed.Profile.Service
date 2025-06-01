using Discussed.Profile.Persistence.Interfaces.Factories;
using Microsoft.Extensions.Configuration;
using Neo4j.Driver;

namespace Discussed.Profile.Persistence.Neo4J.Factories;

public class Neo4JConnectionFactory : INeo4JConnectionFactory, IDisposable
{
    private readonly IDriver _driver;

    public Neo4JConnectionFactory(IConfiguration configuration)
    {
        var config = configuration.GetSection("Neo4J");
        var uri = config["Uri"];
        var username = config["Username"];
        var password = config["Password"];
        
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
    }
    
    public IAsyncSession CreateAsyncSession()
    {
        return _driver.AsyncSession();
    }

    public void Dispose()
    {
        _driver.Dispose();
    }
}