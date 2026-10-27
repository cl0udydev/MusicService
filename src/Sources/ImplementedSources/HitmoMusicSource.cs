using HtmlAgilityPack;

namespace MusicService.Sources;

public class HitmoMusicSource: IMusicSource
{
    private static readonly char[] UrlEndChars = new char[] {'\''};
    public string GeneralNodeArgs { get; } = "//li[contains(@class, 'tracks__item')]";

    public string FormingLink(string userInput) => $"https://ru.hitmoz.org/search?q={userInput.Replace(" ", "%20")}";

    public HtmlNode? GetNameNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//div[contains(@class, 'track__title')]");

    public HtmlNode? GetAuthorNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//div[contains(@class, 'track__desc')]");

    public HtmlNode? GetDurationNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//div[contains(@class, 'track__fulltime')]");

    public string? GetLinkNode(HtmlNode trackNode)
    {
        HtmlNode? linkNode = trackNode.SelectSingleNode(".//a[contains(@class, 'track__download-btn')]");

        if (linkNode == null) return null;

        string link = linkNode.GetAttributeValue("href", "");

        if (link == "") return null;
        
        return $"https://ru.hitmoz.org{link}";
    }

    public string? GetImageLink(HtmlNode trackNode)
    {
        HtmlNode? imgNode = trackNode.SelectSingleNode(".//div[contains(@class, 'track__img')]");

        if (imgNode == null) return null;

        string imgLink = imgNode.GetAttributeValue("style", "");

        if (imgLink == "") return null;

        int startIdx = imgLink.IndexOf("url(") + 5;
        int endIdx = imgLink.IndexOfAny(UrlEndChars, startIdx);

        if (endIdx == -1) return null;

        return imgLink.Substring(startIdx, endIdx - startIdx);
    }
}