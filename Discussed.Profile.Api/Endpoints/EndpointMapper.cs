using Discussed.Profile.Api.Endpoints.Profile;

namespace Discussed.Profile.Api.Endpoints;

public static class EndpointMapper
{
    public static WebApplication MapEndpoints(this WebApplication app)
    {
        app.MapProfileEndpoints();
        return app;
    }
}