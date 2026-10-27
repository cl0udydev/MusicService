using HtmlAgilityPack;

namespace MusicService.Sources;

public interface IMusicSource
{
    string GeneralNodeArgs { get; }
    string FormingLink(string userInput);
    HtmlNode? GetNameNode(HtmlNode mainNode);
    HtmlNode? GetAuthorNode(HtmlNode mainNode);
    HtmlNode? GetDurationNode(HtmlNode mainNode);
    HtmlNode? GetLinkNode(HtmlNode mainNode);
    string? GetImageLink(HtmlNode mainNode);

}