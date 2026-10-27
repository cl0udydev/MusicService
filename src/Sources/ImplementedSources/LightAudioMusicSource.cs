using HtmlAgilityPack;

namespace MusicService.Sources;

public class LightAudioMusicSource: IMusicSource
{
    public string GeneralNodeArgs { get; } = "//div[contains(@class, 'item')]";

    public string FormingLink(string userInput) => $"https://web.ligaudio.ru/mp3/{userInput.Replace(" ", "%20")}";
    
    public HtmlNode? GetNameNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//span[contains(@class, 'title')]");

    public HtmlNode? GetAuthorNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//span[contains(@class, 'autor')]");

    public HtmlNode? GetDurationNode(HtmlNode trackNode) => trackNode.SelectSingleNode(".//span[contains(@class, 'd')]");

    public string? GetLinkNode(HtmlNode trackNode)
    {
        HtmlNode? linkNode = trackNode.SelectSingleNode(".//a[contains(@class, 'down')]");

        if (linkNode == null) return null;

        string link = linkNode.GetAttributeValue("href", "");

        if (link == "") return null;
        
        return link;
    }
        
    public string? GetImageLink(HtmlNode trackNode)
    {
        HtmlNode? imgNode = trackNode.SelectSingleNode("//img[@src]");

        if (imgNode == null) return null;

        string imgLink = imgNode.GetAttributeValue("src", "");

        if (imgLink == "") return null;

        return imgLink;
    }
}