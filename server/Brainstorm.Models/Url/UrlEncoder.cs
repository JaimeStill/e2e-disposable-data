using System.Text.RegularExpressions;

namespace Brainstorm.Models.Url;
public static class UrlEncoder
{
    private static readonly string urlPattern = "[^a-zA-Z0-9-.]";

    public static string Encode(string url) => Encode(url, urlPattern, "-");

    public static string Encode(string url, string pattern, string replace = "")
    {
        var friendlyUrl = Regex.Replace(url, @"\s", replace).ToLower();
        friendlyUrl = Regex.Replace(friendlyUrl, pattern, replace);
        return friendlyUrl;
    }
}