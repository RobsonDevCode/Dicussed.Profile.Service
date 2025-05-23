using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Models.Paging;
using Discussed.Profile.Domain.Models.Paging.OffsetPagination;
using Discussed.Profile.Persistence.Interfaces.Contracts;
using Discussed.Profile.Persistence.Interfaces.Reader;
using HotChocolate.Resolvers;

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

    public async Task<PagedModel<FollowingModel>> GetFollowingPageAsync(Guid userId, OffSetPaginationModel filter,
        CancellationToken cancellationToken)
    {
        var (followings, total) =
            await _followReader.GetFollowingPageAsync(userId, new PagedOptions(filter.Skip, filter.Take),
                cancellationToken);

        if (total == 0)
        {
            return new PagedModel<FollowingModel>();
        }

        return new PagedModel<FollowingModel>
        {
            Items = _mapper.Map(followings),
            Total = total
        };
    }

    public async Task<PagedModel<FollowerModel>> GetFollowerPageAsync(Guid followerId, IEnumerable<string> fields,
        OffSetPaginationModel filter,
        CancellationToken cancellationToken)
    {
        var followerTask = _followReader.GetFollowerPageAsync(followerId, fields,
            new PagedOptions(filter.Skip, filter.Take), cancellationToken);
        
        var totalTask = _followReader.GetTotalFollowersAsync(followerId, cancellationToken);
        
        var tasks = new Task[]{followerTask, totalTask}; 
        await Task.WhenAll(tasks);

        var followers = await followerTask;  
        var total = await totalTask;        
        
        return new PagedModel<FollowerModel>
        {
            Items = _mapper.Map(followers),
            Total = total
        };
    }
}