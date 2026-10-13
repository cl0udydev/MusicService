using System;

namespace MusicService.Domain;

// dataclass for storing tracks information 
public class Track
{
    public string Link { get; }
    public string Name { get; }
    public string Author { get; }
    public TimeSpan Duration { get; }

    public Track(string link, string name, string author, TimeSpan duration)
    {
        Link = link;
        Name = name;
        Author = author;
        Duration = duration;
    }
    public override string ToString()
    {
        return $"[🎵] {Author} — {Name} ({Duration:mm\\:ss})";
    }

}