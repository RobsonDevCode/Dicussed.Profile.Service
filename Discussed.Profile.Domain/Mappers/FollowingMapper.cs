using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Contracts;
using FollowRequest = Discussed.Profile.Api.Contracts.Contracts.FollowRequest;

namespace Discussed.Profile.Domain.Mappers;

public partial class Mapper
{
    public IEnumerable<FollowingModel> Map(IEnumerable<Following> following)
    {
        return following.Select(user => new FollowingModel
        {
            UserId = user.UserId,
            FollowingId = user.FollowingId,
            Username = user.Username
        }).ToList();
    }

    public FollowingResponse[] MapToResponse(IEnumerable<FollowingModel> following)
    {
        return following.Select(user => new FollowingResponse
        {
            UserId = user.UserId,
            FollowingId = user.FollowingId,
            Username = user.Username
        }).ToArray();
    }

    public FollowingType[] MapToResponseType(IEnumerable<FollowingModel> following)
    {
        return following.Select(user => new FollowingType
        {
            UserId = user.UserId,
            FollowingId = user.FollowingId,
            Username = user.Username
        }).ToArray();
    }

    public FollowerType[] MapToResponseType(IEnumerable<FollowerModel> followers)
    {
        return followers.Select(x => new FollowerType
        {
            UserGuid = x.Id,
            Username = x.Username,
            FollowerCount = x.FollowingCount,
            FollowingCount = x.FollowingCount
        }).ToArray();
    }

    public FollowRequestModel MapToDomain(FollowRequest request)
    {
        return new FollowRequestModel
        {
            Id = request.Id,
            UserId = request.UserId,
            UserToFollow = request.UserToFollow,
            IsPrivate = request.IsPrivate
        };
    }

    public DeclineRequestModel MapToDomain(DeclineRequest request)
    {
        return new DeclineRequestModel
        {
            UserRequested = request.UserRequested,
            UserRequesting = request.UserRequesting
        };
    }

    public RemoveFollowRequest Map(DeclineRequestModel request)
    {
        return new RemoveFollowRequest
        {
            UserRequested = request.UserRequested,
            UserRequesting = request.UserRequesting
        };
    }

    public RemoveFollowRequest Map(FollowRequestModel request)
    {
        return new RemoveFollowRequest
        {
            UserRequested = request.UserToFollow,
            UserRequesting = request.UserId
        };
    }

    public Follow Map(FollowRequestModel request, string username)
    {
        return new Follow
        {
            UserId = request.UserId,
            FollowerUserId = request.UserToFollow,
            Username = username
        };
    }

    public IEnumerable<FollowerModel> Map(IEnumerable<Follower> followers)
    {
        return followers.Select(x => new FollowerModel
        {
            Id = x.Id,
            Username = x.Username,
            FollowingCount = x.FollowingCount,
            FollowerCount = x.FollowerCount
        });
    }
}