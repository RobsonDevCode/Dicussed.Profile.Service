using Discussed.Profile.Api.Contracts.Contracts;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Models;
using Discussed.Profile.Domain.Services;
using FluentValidation;
using static Discussed.Profile.Api.Constants.LoggingCategories;

namespace Discussed.Profile.Api.Schemas.Mutations;

[ExtendObjectType(typeof(Mutation))]
public class ProfileMutations
{
    private readonly IFollowingUpsertService _followingUpsertService;
    private readonly IValidator<FollowRequestModel> _validator;
    private readonly IMapper _mapper;
    private readonly ILogger<ProfileMutations> _logger;

    public ProfileMutations(IFollowingUpsertService followingUpsertService, IValidator<FollowRequestModel> validator,
        IMapper mapper, ILogger<ProfileMutations> logger)
    {
        _followingUpsertService = followingUpsertService;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FollowingResponse> FollowUser(FollowRequest request, CancellationToken cancellationToken)
    {
        using var scope = _logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Follow User",
            [Filter] = request 
        });

        return new FollowingResponse
        {
            UserId = default,
            Username = null,
            FollowingId = default
        };
    }
}