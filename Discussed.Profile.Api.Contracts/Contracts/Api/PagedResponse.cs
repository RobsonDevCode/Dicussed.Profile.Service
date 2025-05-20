namespace Discussed.Profile.Api.Contracts.Contracts.Api;

public class PagedResponse<T>
{
    public T[] Items { get; init; }

    public required Pagination Pagination { get; init; }
}