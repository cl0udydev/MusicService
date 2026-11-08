using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MusicService.Audio;
using MusicService.Domain;
using NAudio.Wave;

namespace MusicService.Applic.PlaybackController;

public class PlaybackController
{
    private Track? currentTrack;
    public Track? CurrentTrack => currentTrack;
    public event Action? CurrentTrackChanged;

    private Playlist? playlist;
    public Playlist? Playlist => playlist;
    private int _index;

    public PlaybackState PlaybackState { get; private set; }
    public event Action<PlaybackState>? PlaybackStateChanged;
    private AudioService _audioService;

    private bool isLooped;
    public bool IsLooped => isLooped;

    private bool isShuffle;
    public bool IsShuffle => isShuffle;
    private List<Track>? shuffledTracks;


    public void ChangeLooped()
    {
        if (Playlist != null)
        {
            isLooped = !isLooped;
        }
    }

    
    public void ChangeShuffle()
    {
        if (Playlist != null)
        {
            isShuffle = !isShuffle;
            shuffledTracks = ShuffleIntermediateTracks(Playlist.Tracks);
        }
    }


    public PlaybackController(AudioService audioService)
    {
        _audioService = audioService;
        _audioService.TrackFinished += OnTrackFinished;

        _audioService.PlaybackStateChanged += (state) =>
        {
            PlaybackState = state;
            PlaybackStateChanged?.Invoke(PlaybackState);
        }; 
    }


    private async void OnTrackFinished()
    {
        if (isLooped)
        {
            _audioService.ResetUrl();
            await PlayTrackAsync();
        }
        else if (isShuffle)
        {
            await PlayNextShuffledTrackAsync();
        }
        else
        {
            await PlayNextTrackAsync();
        }
    }


    public void ChangePlaylist(Playlist newPlaylist, int index)
    {
        if (playlist != null)
        {
            if (playlist.Id == newPlaylist.Id && _index == index) return;
        }


        if (newPlaylist.Tracks.Count == 0) 
            throw new ArgumentException($"New playlist must contains at least one track", nameof(newPlaylist));
        if (index < 0) 
            throw new ArgumentOutOfRangeException(nameof(index), $"New playlist index must be more than 0");
        if (index >= newPlaylist.Tracks.Count) 
            throw new ArgumentOutOfRangeException(nameof(index), $"New playlist index out of range");

        StopTrack();

        playlist = newPlaylist;
        _index = index;
        currentTrack = playlist.Tracks[_index];

        if (isShuffle) shuffledTracks = ShuffleIntermediateTracks(playlist.Tracks);

        CurrentTrackChanged?.Invoke();
    }


    public async Task PlayTrackAsync()
    {
        if (currentTrack == null) return;

        await _audioService.PlayAsync(currentTrack.Link);
    }


    public async Task PlayNextTrackAsync()
    {
        if (playlist == null || playlist.Tracks.Count == 0) return;

        _index = (_index + 1) % playlist.Tracks.Count;
        currentTrack = playlist.Tracks[_index];
        CurrentTrackChanged?.Invoke();

        await PlayTrackAsync();
    }


    public async Task PlayPreviousTrackAsync()
    {
        if (playlist == null || playlist.Tracks.Count == 0) return;

        _index = (_index - 1 + playlist.Tracks.Count) % playlist.Tracks.Count;
        currentTrack = playlist.Tracks[_index];
        CurrentTrackChanged?.Invoke();
        
        await PlayTrackAsync();
    }


    private async Task PlayNextShuffledTrackAsync()
    {
        if (shuffledTracks!.Count == 0)
        {
            shuffledTracks = ShuffleIntermediateTracks(playlist!.Tracks);
        }

        var track = shuffledTracks![0];
        shuffledTracks.RemoveAt(0);

        _index = playlist!.Tracks.IndexOf(track);
        currentTrack = playlist.Tracks[_index];

        CurrentTrackChanged?.Invoke();

        await PlayTrackAsync();
    }


    public void StopTrack()
    {
        _audioService.Stop();
    }


    public void PauseTrack()
    {
        _audioService.Pause();
    }


    private List<Track> ShuffleIntermediateTracks(List<Track> tracks)
    {
        var tempTracks = new List<Track>(tracks);

        for (int i = tempTracks.Count - 1; i > 0; i--)
        {
            int j = Random.Shared.Next(i + 1);

            (tempTracks[i], tempTracks[j]) = (tempTracks[j], tempTracks[i]);
        }
        
        return tempTracks;
    }
}