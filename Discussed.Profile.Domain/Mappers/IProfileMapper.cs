using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Mappers;

public interface IProfileMapper
{
    ProfileResponse[] MapToResponse(IEnumerable<ProfileModel> profileResponse);
    IEnumerable<ProfileModel> Map(IEnumerable<Persistence.Interfaces.Contracts.Profile> profiles);
    ProfileModel Map(Persistence.Interfaces.Contracts.Profile profile);
    ProfileResponse MapToResponse(ProfileModel profile);
}