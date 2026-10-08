using System;

namespace MusicService.Domain;

// dataclass for storing tracks information 
public class Track
{
    public string Link { get; }
    public string Name { get; }
    public string Artist { get; }
    public TimeSpan Duration { get; }

    public Track(string link, string name, string artist, TimeSpan duration)
    {
        Link = link;
        Name = name;
        Artist = artist;
        Duration = duration;
    }
}