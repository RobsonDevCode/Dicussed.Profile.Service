using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.Types.Profile;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Contracts;

namespace Discussed.Profile.Domain.Mappers;

public interface IProfileMapper
{
    ProfileResponse[] MapToResponse(IEnumerable<ProfileModel> profileResponse);
    IEnumerable<ProfileModel> Map(IEnumerable<Persistence.Interfaces.Contracts.Profile> profiles);
    ProfileModel Map(Persistence.Interfaces.Contracts.Profile profile);
    ProfileModelV2 Map(ProfileV2 profile);
    ProfileResponse MapToResponse(ProfileModel profile);
    ProfileType MapToResponseType(ProfileModelV2 profile); 
}