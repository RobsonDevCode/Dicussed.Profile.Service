using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Contracts;
using FollowRequest = Discussed.Profile.Api.Contracts.Contracts.FollowRequest;

namespace Discussed.Profile.Domain.Mappers;

public interface IFollowingMapper
{
    IEnumerable<FollowingModel> Map(IEnumerable<Following> following);
    FollowingResponse[] MapToResponse(IEnumerable<FollowingModel> following);
    FollowingType[] MapToResponseType(IEnumerable<FollowingModel> following);

    FollowRequestModel MapToDomain(FollowRequest request);
    Follow Map(FollowRequestModel request, string username);

    DeclineRequestModel MapToDomain(DeclineRequest request);
    
    RemoveFollowRequest Map(DeclineRequestModel request);
    
    RemoveFollowRequest Map(FollowRequestModel request);
    IEnumerable<FollowerModel> Map(IEnumerable<Follower> followers);
    
    FollowerType[] MapToResponseType(IEnumerable<FollowerModel> followers);
}