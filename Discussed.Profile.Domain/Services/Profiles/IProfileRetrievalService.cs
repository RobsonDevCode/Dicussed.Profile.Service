using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Services.Profiles;

public interface IProfileRetrievalService
{
    Task<ProfileModel> GetAsync(Guid userId, CancellationToken cancellationToken);
}