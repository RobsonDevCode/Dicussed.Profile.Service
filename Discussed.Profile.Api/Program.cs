using System.Threading.RateLimiting;
using Discussed.Profile.Api.Endpoints;
using Discussed.Profile.Api.Extensions;
using Discussed.Profile.Api.Extensions.HttpClients;
using Discussed.Profile.Api.Extensions.Logging;
using Discussed.Profile.Api.Handlers;
using Discussed.Profile.Api.Handlers.GraphQL;
using Discussed.Profile.Api.Middleware;
using Discussed.Profile.Api.Schemas;
using Discussed.Profile.Api.Schemas.Mutations;
using Discussed.Profile.Api.Schemas.Queries;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddExceptionHandler<GlobalErrorHandler>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Configuration.AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Logging.ClearProviders();
builder.Services.AddSeqTelemertry(builder.Configuration);
builder.Services.AddDiscussedDependencies();
builder.Services.AddDiscussedAuth(builder.Configuration);
builder.Services.AddProblemDetails();
builder.Services.AddDiscussedSwagger();
builder.Services.AddDiscussedCors();
builder.Services.AddDiscussedHttpClients(builder.Configuration);
builder.Services.AddHttpContextAccessor();

builder.Services.AddGraphQLServer()
    .AddErrorFilter<GraphQLErrorFilter>()
    .UseField<FieldSelectionMiddleware>()
    .AddQueryType<Query>()
    .AddTypeExtension<ProfileQueries>()
    .AddAuthorization()
    .ModifyPagingOptions(options =>
    {
        options.DefaultPageSize = 100;      
        options.MaxPageSize = 500;          
        options.IncludeTotalCount = true;   
    });

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.", token);
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

// First map your minimal API endpoints
app.MapEndpoints();

// Option 1: Direct mapping (preferred for minimal APIs)
app.MapGraphQL();
app.Run();