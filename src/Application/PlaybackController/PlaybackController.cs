using System;
using System.Threading.Tasks;
using MusicService.Audio;
using MusicService.Domain;
using MusicService.Infrastructure;

namespace MusicService.Applic.PlaybackController;

public class PlaybackController
{
    private Track? currentTrack;
    private Playlist? playlist;
    public Playlist? Playlist => playlist;
    private int _index;
    private AudioService _audioService;
    private bool isLooped;

    public PlaybackController(AudioService audioService)
    {
        _audioService = audioService;
        EventBus.TrackFinished += OnTrackFinished;
    }


    private async void OnTrackFinished()
    {
        if (isLooped)
        {
            await PlayTrackAsync();
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
            if (playlist.Id == newPlaylist.Id) return;
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

        await PlayTrackAsync();
    }


    public async Task PlayPreviousTrackAsync()
    {
        if (playlist == null || playlist.Tracks.Count == 0) return;

        _index = (_index - 1 + playlist.Tracks.Count) % playlist.Tracks.Count;
        currentTrack = playlist.Tracks[_index];
        
        await PlayTrackAsync();
    }


    public void ChangeLooped() => isLooped = !isLooped;


    public void StopTrack()
    {
        _audioService.Stop();
    }


    public void PauseTrack()
    {
        _audioService.Pause();
    }

}