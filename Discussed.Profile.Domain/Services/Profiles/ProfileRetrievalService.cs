using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Microsoft.Extensions.Logging;

namespace Discussed.Profile.Domain.Services.Profiles;

public sealed class ProfileRetrievalService : IProfileRetrievalService
{
    private readonly IProfileReader _profileReader;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileRetrievalService> _logger;

    public ProfileRetrievalService(IProfileReader profileReader, IMapper mapper,
        ILogger<ProfileRetrievalService> logger)
    {
        _profileReader = profileReader;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProfileModel> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var profile = await _profileReader.GetByIdAsync(userId, cancellationToken);
        if (profile == null)
            throw new KeyNotFoundException("Profile not found!");

        var mappedProfile = _mapper.Map(profile);
        return mappedProfile;
    }
}