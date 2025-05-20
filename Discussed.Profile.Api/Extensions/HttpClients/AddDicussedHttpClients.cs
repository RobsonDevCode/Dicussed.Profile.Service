using System.Net.Http.Headers;
using Discussed.Profile.Api.Configuration;
using Discussed.Profile.Domain.Clients;

namespace Discussed.Profile.Api.Extensions.HttpClients;

public static class AddDicussedHttpClients
{
    public static IServiceCollection AddDiscussedHttpClients(this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration.GetSection("UserClientOptions").Get<UserClientOptions>();
        if (options == null)
        {
            throw new InvalidOperationException("Failed to retrieve http client information for user client");
        }

        services.AddHttpClient<IUserClient, UserClient>(client =>
        {
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(options.BaseUrl);
        })
        .AddStandardResilienceHandler();
        
        return services;
    }
}