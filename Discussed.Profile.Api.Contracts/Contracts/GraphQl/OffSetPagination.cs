namespace Discussed.Profile.Api.Contracts.Contracts.GraphQl;

public class OffSetPagination
{
    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 100;

    public bool HasNext(int totalCount) => (Skip + Take) < totalCount;
    public bool HasPrevious => Skip > 0;
}