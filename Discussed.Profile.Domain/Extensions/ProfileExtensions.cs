namespace Discussed.Profile.Domain.Extensions;

public static class ProfileExtensions
{
    public static string ToFollowDisplay(this int followers)
    {
        switch (followers)
        {
            case < 1000:
                return followers.ToString();

            case < 10000:
            {
                // Format: 1.2k for 1,234
                var value = followers / 1000.0m;
                return value.ToString("0.#") + "k";
            }

            case < 1000000:
            {
                // Format: 12k, 123k for 12,345 and 123,456
                var value = followers / 1000;
                return value + "k";
            }

            case < 10000000:
            {
                // Format: 1.2M for 1,234,567
                var value = followers / 1000000.0m;
                return value.ToString("0.#") + "M";
            }

            case < 1000000000:
            {
                // Format: 12M, 123M for 12,345,678 and 123,456,789
                var value = followers / 1000000;
                return value + "M";
            }

            default:
            {
                var value = followers / 1000000000.0m;
                return value.ToString("0.#") + "B";
            }
        }
    }  
    
    public static string? ToFollowDisplay(this int? followers)
    {
        if (followers is null)
        {
            return null;
        }
        
        switch (followers.Value)
        {
            case < 1000:
                return followers.ToString();

            case < 10000:
            {
                // Format: 1.2k for 1,234
                var value = followers.Value / 1000.0m;
                return value.ToString("0.#") + "k";
            }

            case < 1000000:
            {
                // Format: 12k, 123k for 12,345 and 123,456
                var value = followers.Value / 1000;
                return value + "k";
            }

            case < 10000000:
            {
                // Format: 1.2M for 1,234,567
                var value = followers.Value / 1000000.0m;
                return value.ToString("0.#") + "M";
            }

            case < 1000000000:
            {
                // Format: 12M, 123M for 12,345,678 and 123,456,789
                var value = followers.Value / 1000000;
                return value + "M";
            }

            default:
            {
                var value = followers.Value / 1000000000.0m;
                return value.ToString("0.#") + "B";
            }
        }
    }
}