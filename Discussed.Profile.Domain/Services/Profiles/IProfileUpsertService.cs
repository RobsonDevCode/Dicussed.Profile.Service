using Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;
using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Services.Profiles;

public interface IProfileUpsertService
{
    Task<Persistence.Interfaces.Contracts.Profile> CreateProfileAsync(CreateProfileInput request, IEnumerable<string>? fields = null,
        CancellationToken cancellationToken = default);
}