using Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;
using Discussed.Profile.Domain.Clients;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Writer;
using Microsoft.Extensions.Logging;

namespace Discussed.Profile.Domain.Services.Profiles;

public sealed class ProfileUpsertService : IProfileUpsertService
{
    private readonly IProfileWriter _profileWriter;
    private readonly IUserClient _userClient;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileUpsertService> _logger;

    public ProfileUpsertService(IProfileWriter profileWriter, IUserClient userClient, IMapper mapper,
        ILogger<ProfileUpsertService> logger)
    {
        _profileWriter = profileWriter;
        _userClient = userClient;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<Persistence.Interfaces.Contracts.Profile> CreateProfileAsync(CreateProfileInput request, IEnumerable<string>? fields = null,
        CancellationToken cancellationToken = default)
    {
        var username = (await _userClient.GetAsync(request.UserId, fields, cancellationToken)).Username;
        if (username is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var result = _mapper.MapToPersistance(request, username);
        await _profileWriter.CreateProfile(result, cancellationToken);

        return result;
    }
}