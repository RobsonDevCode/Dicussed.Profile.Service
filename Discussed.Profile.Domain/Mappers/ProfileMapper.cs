using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Domain.Extensions;
using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Mappers;

public partial class Mapper
{
    public IEnumerable<ProfileModel> Map(IEnumerable<Persistence.Interfaces.Contracts.Profile> profiles)
    {
        return profiles.Select(profile => new ProfileModel
        {
            UserId = profile.UserId, 
            FollowerCount = profile.FollowerCount.ToFollowDisplay(), 
            FollowingCount = profile.FollowingCount.ToFollowDisplay()
        }).ToList();
    }

    public ProfileModel Map(Persistence.Interfaces.Contracts.Profile profile)
    {
        return new ProfileModel
        {
            UserId = profile.UserId,
            FollowerCount = profile.FollowerCount.ToFollowDisplay(),
            FollowingCount = profile.FollowingCount.ToFollowDisplay()
        };
    }

    public ProfileResponse MapToResponse(ProfileModel profile)
    {
        return new ProfileResponse
        {
            UserId = profile.UserId,
            FollowerCount = profile.FollowerCount,
            FollowingCount = profile.FollowingCount
        };
    }
}