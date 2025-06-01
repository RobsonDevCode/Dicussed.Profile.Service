using Discussed.Profile.Domain.Models;

namespace Discussed.Profile.Domain.Clients;

public interface IUserClient
{
    Task<UserModel> GetAsync(Guid userId, CancellationToken cancellationToken);

    Task<UserModel?> GetAsync(Guid userId, IEnumerable<string> fields,
        CancellationToken cancellationToken);
}