using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.Types.Profile;
using Discussed.Profile.Domain.Extensions;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Domain.Mappers;

public partial class Mapper
{
    public IEnumerable<ProfileModel> Map(IEnumerable<Persistence.Interfaces.Contracts.Profile> profiles)
    {
        return profiles.Select(profile => new ProfileModel
        {
            UserId = profile.UserId,
            FollowerCount = profile.FollowerCount.ToFollowDisplay(),
            FollowingCount = profile.FollowingCount.ToFollowDisplay(),
            Bio = null,
            Private = false
        }).ToList();
    }

    public ProfileModel Map(Persistence.Interfaces.Contracts.Profile profile)
    {
        return new ProfileModel
        {
            UserId = profile.UserId,
            FollowerCount = profile.FollowerCount.ToFollowDisplay(),
            FollowingCount = profile.FollowingCount.ToFollowDisplay(),
            Bio = null,
            Private = false
        };
    }

    public ProfileModelV2 Map(ProfileV2 profile)
    {
        return new ProfileModelV2
        {
            Id = profile.UserId,
            UserName = profile.Username,
            FollowerCount = !profile.FollowerCount.HasValue ? null : profile.FollowerCount.ToFollowDisplay(),
            FollowingCount = !profile.FollowingCount.HasValue ? null : profile.FollowingCount.ToFollowDisplay(),
            Bio = profile.Bio,
            Private = profile.Private
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

    public ProfileType MapToResponseType(ProfileModelV2 profile)
    {
        return new ProfileType
        {
            UserId = profile.Id,
            FollowerCount = profile.FollowerCount,
            FollowingCount = profile.FollowingCount,
            Bio = profile.Bio,
            Private = profile.Private,
        };
    }

    public ProfileType MapToResponseType(Persistence.Interfaces.Contracts.Profile profile)
    {
        return new ProfileType
        {
            UserId = profile.UserId,
            Bio = profile.Bio,
            Private = profile.Private,
            FollowerCount = profile.FollowerCount.ToFollowDisplay(),
            FollowingCount = profile.FollowingCount.ToFollowDisplay(),
        };
    }

    public Persistence.Interfaces.Contracts.Profile MapToPersistance(CreateProfileInput request, string userName)
    {
        return new Persistence.Interfaces.Contracts.Profile
        {
            UserId = request.UserId,
            Bio = request.Bio,
            Private = request.Private,
            Username = userName,
            FollowerCount = 0,
            FollowingCount = 0,
        };
    }
}