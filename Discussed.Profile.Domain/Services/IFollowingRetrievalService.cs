using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Models.Paging;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;

namespace Discussed.Profile.Domain.Services;

public interface IFollowingRetrievalService
{
    Task<PagedModel<FollowingModel>> GetFollowingPageAsync(Guid userId, OffSetPaginationModel filter,
        CancellationToken cancellationToken);

    Task<PagedModel<FollowerModel>> GetFollowerPageAsync(Guid followerId, IEnumerable<string> fields,
        OffSetPaginationModel filter, CancellationToken cancellationToken);
}