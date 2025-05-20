using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Models.Paging;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;

namespace Discussed.Profile.Domain.Services;

public interface IFollowingRetrievalService
{
    Task<PagedFollowingModel> GetPageAsync(Guid userId, OffSetPaginationModel filter, CancellationToken cancellationToken);
}