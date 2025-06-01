using Discussed.Profile.Api.Configuration;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Discussed.Profile.Api.Extensions.Logging;

public static class LoggerBuilderExtensions
{
    public static IServiceCollection AddSeqTelemertry(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection("SeqSettings").Get<SeqSettings>()
            ?? throw new InvalidOperationException("Missing SeqTelemertry configuration.");
        
        services.AddOpenTelemetry().ConfigureResource(r => r.AddService("Discussed.CreateProfileInput.Api"))
            .WithTracing(x =>
            {
                x.AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                x.AddOtlpExporter(y =>
                {
                    y.Endpoint = new Uri(options.Uri);
                    y.Protocol = OtlpExportProtocol.HttpProtobuf;
                    y.Headers = options.Headers;
                });
            });
        
        return services;
    }
}