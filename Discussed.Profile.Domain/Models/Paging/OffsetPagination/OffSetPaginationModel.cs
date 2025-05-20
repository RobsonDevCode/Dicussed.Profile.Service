namespace Discussed.Profile.Domain.Models.Paging.OffsetPagination;

public record OffSetPaginationModel
{
    public OffSetPaginationModel(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    public int Skip { get; init; } = 0;
    public int Take { get; init; } = 100;
}