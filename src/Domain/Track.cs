using System;

namespace MusicService.Domain;

// dataclass for storing tracks information 
public record class Track
{
    public string Link { get; }
    public string Name { get; }
    public string Author { get; }
    public TimeSpan Duration { get; }
    public string ImageLink { get; }
    public string LyricsLink { get; }

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