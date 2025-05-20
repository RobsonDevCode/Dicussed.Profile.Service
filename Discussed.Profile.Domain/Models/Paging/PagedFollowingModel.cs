namespace Discussed.Profile.Domain.Models.Paging;

public record PagedFollowingModel
{

    public IEnumerable<FollowingModel> Following { get; init; } = [];

    public int Total { get; init; } = 0;
}