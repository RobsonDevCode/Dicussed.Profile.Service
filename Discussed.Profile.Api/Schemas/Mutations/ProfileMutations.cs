using Discussed.Profile.Api.Constants;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;
using Discussed.Profile.Api.Contracts.Contracts.GraphQl.Types.Profile;
using Discussed.Profile.Api.Extensions.ResolverFields;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Domain.Services.Profiles;
using FluentValidation;
using HotChocolate.Authorization;
using HotChocolate.Resolvers;
using static Discussed.Profile.Api.Constants.LoggingCategories;

namespace Discussed.Profile.Api.Schemas.Mutations;

[ExtendObjectType(typeof(Mutation))]
public sealed class ProfileMutations
{
    [GraphQLName("createProfile")]
    [Authorize]
    public async Task<ProfileType> CreateProfileAsync(CreateProfileInput request,
        IResolverContext resolverContext,
        [Service] IProfileUpsertService profileUpsertService,
        [Service] IMapper mapper,
        [Service] ILoggerFactory loggerFactory,
        [Service] IValidator<CreateProfileInput> validator,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger(LoggingConstants.ProfileResolver);
        using var scope = logger.BeginScope(new Dictionary<string, object>
        {
            [Operation] = "Create Profile",
            [User] = request.UserId,
            [Filter] = request
        });

        var validation = await validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
        {
            var formattedErrors = string.Join(" | ", validation.Errors);
            throw new ValidationException(formattedErrors);
        }

        var fields = resolverContext.GetSelectedFields();

        var response = await profileUpsertService.CreateProfileAsync(request, fields, cancellationToken);
        var result = mapper.MapToResponseType(response);
        return result;
    }
}