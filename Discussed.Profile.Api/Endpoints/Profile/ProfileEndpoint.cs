using Discussed.Profile.Api.Constants;
using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Services;
using Discussed.Profile.Domain.Services.Profiles;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static Discussed.Profile.Api.Constants.LoggingCategories;

namespace Discussed.Profile.Api.Endpoints.Profile;

internal static class ProfileEndpoint
{
    internal static WebApplication MapProfileEndpoints(this WebApplication app)
    {
        var profileGroup = app.MapGroup("profile")
            .RequireAuthorization()
            .WithTags(ApiTags.Profile);

        profileGroup.MapGet("{userId}", GetProfileById);

        profileGroup.MapPost("follow", FollowUser);
        profileGroup.MapPost("accept-request", AcceptFollowRequest);

        profileGroup.MapDelete("{userRequesting}/{userRequested}", DeclineFollowRequest);


        app.MapGet("Generate-Follower", GenerateFollowing);
        return app;
    }

    private static async Task<Ok<ProfileResponse>> GetProfileById([FromQuery] Guid userId,
        [FromServices] IProfileRetrievalService profileRetrievalService,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileEndpoint);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Profile By Id",
            [User] = userId
        });

        var result = await profileRetrievalService.GetAsync(userId, cancellationToken);
        var mappedResult = mapper.MapToResponse(result);
        logger.LogInformation("Get Profile returned successfully!");

        return TypedResults.Ok(mappedResult);
    }

    private static async Task<Results<Ok, BadRequest<string>>> FollowUser([FromBody] FollowRequest request,
        [FromServices] IFollowingUpsertService followingUpsertService,
        [FromServices] IValidator<FollowRequestModel> validator,
        [FromServices] IMapper mapper,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileEndpoint);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Follow User",
            [User] = request.UserId,
            [Filter] = request,
        });

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var formatedErrors = string.Join(" | ", validationResult.Errors);
            return TypedResults.BadRequest(formatedErrors);
        }

        var mappedRequest = mapper.MapToDomain(request);
        await followingUpsertService.FollowById(mappedRequest, cancellationToken);

        logger.LogInformation("Follow User returned successfully!");
        return TypedResults.Ok();
    }

    private static async Task<Ok> AcceptFollowRequest([FromBody] FollowRequest request,
        [FromServices] IFollowingUpsertService followingUpsertService,
        [FromServices] ILoggerFactory loggerFactory,
        [FromServices] IValidator<FollowRequestModel> validator,
        [FromServices] IMapper mapper,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileEndpoint);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Accept Follow Request",
            [User] = request.UserId,
            [Filter] = request
        });

        logger.LogInformation("Attempting to accept follow request");

        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var formatedErrors = string.Join(" | ", validationResult.Errors);
            throw new ValidationException(formatedErrors);
        }

        var mappedRequest = mapper.MapToDomain(request);
        await followingUpsertService.AcceptRequestAsync(mappedRequest, cancellationToken);

        logger.LogInformation("Request accepted successfully!");

        return TypedResults.Ok();
    }

    private static async Task<Ok> DeclineFollowRequest([FromQuery] Guid userRequesting,
        [FromQuery] Guid userRequested,
        [FromServices] IFollowingRemovalService followingRemovalService,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileEndpoint);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "RemoveFollowRequest Request",
            [User] = userRequesting
        });

        logger.LogInformation("Attempting to decline request");

        var request = new DeclineRequestModel
        {
            UserRequested = userRequested,
            UserRequesting = userRequesting
        };

        await followingRemovalService.DeclineFollowRequest(request, cancellationToken);
        return TypedResults.Ok();
    }

    private static async Task<Created> GenerateFollowing(Guid userId,
        int count,
        [FromServices] IFollowingUpsertService followingUpsertService)
    {
        for (var i = 0; i < count; i++)
        {
            var followRequest = new FollowRequestModel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                UserToFollow = Guid.NewGuid(),
                IsPrivate = false,
            };
            
            await followingUpsertService.FollowById(followRequest, CancellationToken.None);
        }
      
        return TypedResults.Created();
    }
}