namespace Discussed.Profile.Api.Contracts.Contracts.Api;

public record Pagination
{
    public int Size { get; init; } = 100;

    public int Page { get; init; } = 1;

    public required int Total { get; init; }
}