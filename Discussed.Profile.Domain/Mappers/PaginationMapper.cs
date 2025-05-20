using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.Api;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Models.Paging;

namespace Discussed.Profile.Domain.Mappers;

public partial class Mapper
{
    public ProfileResponse[] MapToResponse(IEnumerable<ProfileModel> profileResponse)
    {
        return profileResponse.Select(profile => new ProfileResponse
        {
            UserId = profile.UserId,
            FollowingCount = profile.FollowingCount,
            FollowerCount = profile.FollowerCount
        }).ToArray();
    }
}