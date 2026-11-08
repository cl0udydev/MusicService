using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MusicService.Domain;

// tracks playlist class 
public class Playlist
{
    public int Id { get; }
    public string Name { get; private set; }
    
    [JsonInclude]
    [JsonPropertyName("Tracks")]
    private List<Track> _tracks;

    [JsonIgnore]
    public List<Track> Tracks => _tracks; // kludge that weakens encapsulation

    [JsonConstructor]
    public Playlist(int id)
    {
        Id = id;
        Name = $"playlist #{Id}";
        _tracks = new();
    }


    public void AddTrack(Track track)
    {
        if (_tracks.Contains(track))
        {
            return;
        }
        _tracks.Add(track);
    }


    public void RemoveTrack(Track track)
    {
        if (!_tracks.Contains(track))
        {
            throw new ArgumentException(nameof(track));
        }
        _tracks.Remove(track);
    }


    public void ReplaceTracks(List<Track> tracks)
    {
        _tracks.Clear();

        foreach (Track track in tracks)
        {
            AddTrack(track);    
        }
    }


    public void ChangeName(string name)
    {
        Name = name;
    }
}