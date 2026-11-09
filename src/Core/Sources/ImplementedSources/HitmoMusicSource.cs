using HtmlAgilityPack;

namespace MusicService.Core.Sources;

public class HitmoMusicSource: IMusicSource
{
    private static readonly char[] UrlEndChars = new char[] {'\''};
    public string GeneralNodeArgs { get; } = "//div[contains(@class, 'track-item')]";

    public string FormingLink(string userInput) => $"https://rus.hitmos.fm/search?q={userInput.Replace(" ", "%20")}";

    public HtmlNode? GetNameNode(HtmlNode trackNode) => trackNode.SelectSingleNode(
        ".//a[contains(@class, 'track-item__truncate track-item__title track-item__link track-item__link--above-play')]");

    public HtmlNode? GetAuthorNode(HtmlNode trackNode) => trackNode.SelectSingleNode(
        ".//a[contains(@class, 'track-item__truncate track-item__artist track-item__link track-item__link--above-play')]");

    public HtmlNode? GetDurationNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//span[contains(@class, 'track-item__duration')]");

    public string? GetLink(HtmlNode trackNode)
    {
        HtmlNode? linkNode = trackNode.SelectSingleNode(
            ".//a[contains(@class, 'track-item__truncate track-item__title track-item__link track-item__link--above-play')]");

        if (linkNode == null) return null;

        string link = linkNode.GetAttributeValue("href", "");

        if (link == "") return null;
        
        return $"https://rus.hitmos.fm{link}";
    }

    public string? GetImageLink(HtmlNode trackNode)
    {
        HtmlNode? imgNode = trackNode.SelectSingleNode(".//img[contains(@class, 'track-item__cover')]");

        if (imgNode == null) return null;

        string imgLink = imgNode.GetAttributeValue("src", "");

        if (imgLink == "") return null;

        return imgLink;
    }
}