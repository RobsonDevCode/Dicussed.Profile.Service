using Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;
using FluentValidation;

namespace Discussed.Profile.Api.Validation;

public class CreateProfileValidator : AbstractValidator<CreateProfileInput>
{
    public CreateProfileValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.Bio).MinimumLength(2).MaximumLength(100);
    }
}