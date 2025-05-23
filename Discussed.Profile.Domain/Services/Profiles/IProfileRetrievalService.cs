using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Services.Profiles;

public interface IProfileRetrievalService
{
    Task<ProfileModel> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
}