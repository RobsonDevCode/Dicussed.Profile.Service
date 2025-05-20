using Discussed.Profile.Api.Constants;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;
using Discussed.Profile.Domain.Services;
using HotChocolate.Authorization;
using HotChocolate.Types.Pagination;
using static Discussed.Profile.Api.Constants.LoggingCategories;

namespace Discussed.Profile.Api.Schemas.Queries;

[ExtendObjectType(typeof(Query))]
public sealed class ProfileQueries
{
    [UseOffsetPaging(IncludeTotalCount = true)]
    [Authorize]
    public async Task<CollectionSegment<FollowingType>> GetPagedAsync(
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

        var response = await followingRetrievalService.GetPageAsync(id,
            new OffSetPaginationModel(paginationOptions.Skip, paginationOptions.Take), cancellationToken);

        var result = mapper.MapToResponseType(response.Following);
        var hasNextPage = paginationOptions.HasNext(response.Total);
        var hasPreviousPage = paginationOptions.HasPrevious;

        logger.LogInformation("Successfully retrieved Paged Followers");

        return new CollectionSegment<FollowingType>(result,
            new CollectionSegmentInfo(hasNextPage, hasPreviousPage), response.Total);
    }
}