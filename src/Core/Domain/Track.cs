using System;
using System.Text.Json.Serialization;

namespace MusicService.Core.Domain;

// dataclass for storing tracks information 
public record class Track
{
    public string Link { get; init; }
    public string Name { get; init; }
    public string Author { get; init; }
    public TimeSpan Duration { get; init; }
    public string ImageLink { get; init; }
    public string LyricsLink { get; init; }

    [JsonConstructor]
    public Track(string link, string name, string author, TimeSpan duration, string imageLink, string lyricsLink)
    {
        Link = link;
        Name = name;
        Author = author;
        Duration = duration;
        ImageLink = imageLink;
        LyricsLink = lyricsLink;
    }
    public override string ToString()
    {
        return $"[🎵] {Author} — {Name} ({Duration:mm\\:ss})";
    }

}