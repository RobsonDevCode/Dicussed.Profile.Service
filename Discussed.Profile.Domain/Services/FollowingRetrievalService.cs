using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Models.Paging;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;
using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Reader;

namespace Discussed.Profile.Domain.Services;

public sealed class FollowingRetrievalService : IFollowingRetrievalService
{
    private readonly IFollowReader _followReader;
    private readonly IMapper _mapper;

    public FollowingRetrievalService(IFollowReader followReader, IMapper mapper)
    {
        _followReader = followReader;
        _mapper = mapper;
    }

    public async Task<PagedFollowingModel> GetPageAsync(Guid userId, OffSetPaginationModel filter,
        CancellationToken cancellationToken)
    {
        var (followings, total) =
            await _followReader.GetPageAsync(userId, new PagedOptions(filter.Skip, filter.Take), cancellationToken);

        if (total == 0)
        {
            return new PagedFollowingModel();
        }

        return new PagedFollowingModel
        {
            Following = _mapper.Map(followings),
            Total = total
        };
    }

}