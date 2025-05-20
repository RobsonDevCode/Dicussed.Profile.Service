namespace Discussed.Profile.Persistence.Interfaces.Contracts;

public class PagedOptions
{
    public PagedOptions(int skip, int take)
    {
        Skip = skip;
        Take = take;
    }

    /// <summary>
    /// The current number of values to skip
    /// defaults to 0 if no value given
    /// </summary>
    public int Skip { get; set; } = 0;
    
    /// <summary>
    /// The maximum number of data to return.
    /// defaults to 100 if no value given
    /// </summary>
    public int Take { get; set; } = 100;
}