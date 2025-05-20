using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Persistence.Interfaces.Writer;
using Microsoft.Extensions.Logging;

namespace Discussed.Profile.Domain.Services;

public sealed class FollowingRemovalService : IFollowingRemovalService
{
    private readonly IFollowWriter _followWriter;
    private readonly IMapper _mapper;
    private readonly ILogger<FollowingRemovalService> _logger;

    public FollowingRemovalService(IFollowWriter followWriter, IMapper mapper, ILogger<FollowingRemovalService> logger)
    {
        _followWriter = followWriter;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task DeclineFollowRequest(DeclineRequestModel request, CancellationToken cancellationToken)
    {
        var mappedRequest = _mapper.Map(request);
        await _followWriter.RemoveFollowRequest(mappedRequest, cancellationToken);
    }
}