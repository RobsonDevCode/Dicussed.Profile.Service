using Discussed.Profile.Api.Constants;
using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl;
using Discussed.Profile.Api.Extensions.ResolverFields;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;
using Discussed.Profile.Domain.Services;
using Discussed.Profile.Domain.Services.Profiles;
using Discussed.Profile.Persistence.Interfaces;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using HotChocolate.Types.Pagination;
using static Discussed.Profile.Api.Constants.LoggingCategories;

namespace Discussed.Profile.Api.Schemas.Queries;

[ExtendObjectType(typeof(Query))]
public sealed class ProfileQueries
{
    [GraphQLName("pagedFollowing")]
    [UseOffsetPaging(IncludeTotalCount = true)]
    [Authorize]
    public async Task<CollectionSegment<FollowingType>> GetPagedFollowingAsync(
        Guid id,
        OffSetPagination paginationOptions,
        [Service] IFollowingRetrievalService followingRetrievalService,
        [Service] IMapper mapper,
        [Service] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileResolver);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Paged Profile Query",
            [Filter] = paginationOptions,
            [User] = id
        });

        logger.LogInformation("Attempting to get Paged Followers");

        var response = await followingRetrievalService.GetFollowingPageAsync(id,
            new OffSetPaginationModel(paginationOptions.Skip, paginationOptions.Take)
            , cancellationToken);

        var result = mapper.MapToResponseType(response.Items);
        var hasNextPage = paginationOptions.HasNext(response.Total);
        var hasPreviousPage = paginationOptions.HasPrevious;

        logger.LogInformation("Successfully retrieved Paged Following ");

        return new CollectionSegment<FollowingType>(result,
            new CollectionSegmentInfo(hasNextPage, hasPreviousPage), response.Total);
    }

    [GraphQLName("pagedFollowers")]
    [UseOffsetPaging(IncludeTotalCount = true)]
    [Authorize]
    public async Task<CollectionSegment<FollowerType>> GetPagedFollowersAsync(Guid id,
        OffSetPagination paginationOptions,
        IResolverContext resolverContext,
        [Service] IFollowingRetrievalService followingRetrievalService,
        [Service] IMapper mapper,
        [Service] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileResolver);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Get Paged Followers Query",
            [Filter] = paginationOptions,
            [User] = id
        });

        logger.LogInformation("Attempting to get Paged Followers");

        var fields = resolverContext.GetItemsFields();

        var response = await followingRetrievalService.GetFollowerPageAsync(id, fields,
            new OffSetPaginationModel(paginationOptions.Skip, paginationOptions.Take), cancellationToken);

        var result = mapper.MapToResponseType(response.Items);
        var hasNextPage = paginationOptions.HasNext(response.Total);
        var hasPreviousPage = paginationOptions.HasPrevious;

        logger.LogInformation("Successfully retrieved Paged Followers");

        return new CollectionSegment<FollowerType>(result, new CollectionSegmentInfo(hasNextPage, hasPreviousPage),
            response.Total);
    }

    [Authorize]
    public async Task<ProfileResponse> GetByIdAsync(Guid id,
        [Service] IProfileRetrievalService profileRetrievalService,
        IResolverContext resolverContext,
        [Service] IMapper mapper,
        [Service] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileResolver);
        using var scope = logger.BeginScope(new Dictionary<string, object>

        {
            [Operation] = "Get Profile By Id Query"
        });

        var profile = await profileRetrievalService.GetByIdAsync(id, cancellationToken);
        logger.LogInformation("Successfully retrieved Profile");

        return mapper.MapToResponse(profile);
    }
}