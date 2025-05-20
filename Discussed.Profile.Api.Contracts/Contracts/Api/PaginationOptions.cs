namespace Discussed.Profile.Api.Contracts.Contracts.Api;

public record PaginationOptions
{
    public PaginationOptions(int page, int size)
    {
        Page = page;
        Size = size;
    }

    /// <summary>
    /// The current page number of data to retrieve, -1 based
    /// defaults to 1 if no value given
    /// </summary>
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// The maximum number of data to return.
    /// defaults to 100 if no value given
    /// </summary>
    public int Size { get; set; } = 100;
}