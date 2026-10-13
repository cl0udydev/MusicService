namespace MusicService.Sources;

public readonly struct TrackSourceParseRule
{
    public string HtmlClass { get; }
    public string HtmlTag { get; }
    
    public TrackSourceParseRule(string htmlClass, string htmlTag)
    {
        HtmlClass = htmlClass;
        HtmlTag = htmlTag;
    }
}