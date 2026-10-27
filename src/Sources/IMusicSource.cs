using HtmlAgilityPack;

namespace MusicService.Sources;

public interface IMusicSource
{
    string GeneralNodeArgs { get; }
    string FormingLink(string userInput);
    HtmlNode? GetNameNode(HtmlNode trackNode);
    HtmlNode? GetAuthorNode(HtmlNode trackNode);
    HtmlNode? GetDurationNode(HtmlNode trackNode);
    HtmlNode? GetLinkNode(HtmlNode trackNode);
    string? GetImageLink(HtmlNode trackNode);

}