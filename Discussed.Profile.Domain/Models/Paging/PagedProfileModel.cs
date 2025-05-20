namespace Discussed.Profile.Domain.Models.Paging;

public record PagedProfileModel
{
    public IEnumerable<ProfileModel> Profiles { get; init; } = [];

    public int Total { get; init; } = 0;
}