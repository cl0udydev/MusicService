using System.Collections.Generic;

namespace MusicService.Sources;

// music site/source class
public abstract class MusicSource
{
    public string UrlPrefix { get; set; } // prefix for track link
    public Dictionary<TrackSourceType, TrackSourceParseRule> ParseTags { get; protected set; }

    public MusicSource() // constructor-stub
    {
        UrlPrefix = "";
        ParseTags = new();
    }

    public abstract string FormingLink(string userInput); 
}