namespace Discussed.Profile.Domain.Models.Paging;

public record PagedModel<T> 
{

    public IEnumerable<T> Items { get; init; } = [];

    public int Total { get; init; } = 0;
}