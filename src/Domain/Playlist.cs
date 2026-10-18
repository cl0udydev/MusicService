using System;
using System.Collections.Generic;

namespace MusicService.Domain;

// tracks playlist class 
public class Playlist
{
    public int Id { get; }
    private readonly List<Track> _tracks;
    public IReadOnlyList<Track> Tracks => _tracks;


    public Playlist(int id)
    {
        Id = id;
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
}