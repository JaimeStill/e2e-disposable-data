using System.Text.RegularExpressions;

namespace Brainstorm.Extensions;
public static class BrainstormExtensions
{
    private static readonly string urlPattern = "[^a-zA-Z0-9-.]";

    public static IQueryable<T> SetupSearch<T>(
        this IQueryable<T> values,
        string search,
        Func<IQueryable<T>, string, IQueryable<T>> action,
        char split = '|'
    )
    {
        if (search.Contains(split))
        {
            var searches = search.Split(split);

            foreach (var s in searches)
            {
                values = action(values, s.Trim());
            }

            return values;
        }
        else
            return action(values, search);
    }

    public static string UrlEncode(this string url) => url.UrlEncode(urlPattern, "-");

    public static string UrlEncode(this string url, string pattern, string replace = "")
    {
        var friendlyUrl = Regex.Replace(url, @"\s", "-").ToLower();
        friendlyUrl = Regex.Replace(friendlyUrl, pattern, replace);
        return friendlyUrl;
    }
}
