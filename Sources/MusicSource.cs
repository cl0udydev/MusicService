using System.Collections.Generic;

namespace MusicService.Sources;

// music site/source class
public abstract class MusicSource
{
    public string SourceLink { get; set; }
    public Dictionary<TrackSourceType, TrackSourceParseRule> ParseTags { get; protected set; }

    public MusicSource() // constructor-stub
    {
        SourceLink = "";
        ParseTags = new();
    }

    public abstract string FormingLink(string userInput); 
}