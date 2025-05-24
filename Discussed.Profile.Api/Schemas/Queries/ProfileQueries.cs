using Discussed.Profile.Api.Constants;
using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.Types.Profile;
using Discussed.Profile.Api.Extensions.ResolverFields;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;
using Discussed.Profile.Domain.Services;
using Discussed.Profile.Domain.Services.Profiles;
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

    public async Task<ProfileType> GetByIdAsync(Guid id,
        IResolverContext resolverContext,
        [Service] IProfileRetrievalService profileRetrievalService,
        [Service] IMapper mapper,
        [Service] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileResolver);
        using var scope = logger.BeginScope(new Dictionary<string, object>

        {
            [Operation] = "Get Profile By Id Query"
        });

        var fields = resolverContext.GetSelectedFields();
        if (fields is null || fields.Count is 0)
        {
            throw new BadHttpRequestException("No fields selected");
        }

        var profile = await profileRetrievalService.GetByIdAsync(id, fields, cancellationToken);
        logger.LogInformation("Successfully retrieved Profile");

        return mapper.MapToResponseType(profile);
    }
}