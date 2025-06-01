namespace Discussed.Profile.Api.Contracts.Contracts.GraphQl.MutationsTypes;

public record CreateProfileInput
{
    public required Guid UserId { get; init; }
    public required string Bio { get; init; } = "";
    public required bool Private { get; init; } = true;
}