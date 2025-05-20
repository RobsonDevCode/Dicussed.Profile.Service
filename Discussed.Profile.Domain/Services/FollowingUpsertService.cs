using Discussed.Profile.Domain.Clients;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Discussed.Profile.Persistence.Interfaces.Writer;

namespace Discussed.Profile.Domain.Services;

public sealed class FollowingUpsertService : IFollowingUpsertService
{
    private readonly IFollowWriter _followerWriter;
    private readonly IProfileReader _profileReader;
    private readonly IUserClient _userClient;
    private readonly IMapper _mapper;

    public FollowingUpsertService(IProfileReader profileReader,
        IFollowWriter followerWriter,
        IUserClient userClient,
        IMapper mapper)
    {
        _profileReader = profileReader;
        _followerWriter = followerWriter;
        _userClient = userClient;
        _mapper = mapper;
    }
    public async Task FollowById(FollowRequestModel request, CancellationToken cancellationToken)
    {
        var profile = await _profileReader.GetByIdAsync(request.UserId, cancellationToken);
        if (profile is null)
        {
            throw new KeyNotFoundException($"User {request.UserId} not found!");
        }
        
        var user = await _userClient.GetAsync(request.UserId, cancellationToken);
        var mappedRequest = _mapper.Map(request, user.Username);

        if (profile.Private)
        {
            await _followerWriter.AddFollowRequest(mappedRequest, cancellationToken);
        }

        await _followerWriter.FollowUser(mappedRequest, cancellationToken);
    }

    public async Task AcceptRequestAsync(FollowRequestModel request, CancellationToken cancellationToken)
    {
        var user = await _userClient.GetAsync(request.UserId, cancellationToken);
        var mappedRequest = _mapper.Map(request, user.Username);

        await _followerWriter.FollowUser(mappedRequest, cancellationToken);
        
        var removeFollowRequest = _mapper.Map(request);
        await _followerWriter.RemoveFollowRequest(removeFollowRequest, cancellationToken);
    }
}