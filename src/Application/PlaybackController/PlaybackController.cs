using MusicService.Audio;
using MusicService.Domain;
using MusicService.Infrastructure;
using NAudio.Wave;
using System;

namespace MusicService.Applic;

public class PlaybackController
{
    private Track? currentTrack;
    private Playlist? playlist;
    private int _index;
    private AudioService _audioService;
    private bool isLooped;

    public PlaybackController(AudioService audioService)
    {
        _audioService = audioService;
        EventBus.TrackFinished += OnTrackFinished;
    }


    private void OnTrackFinished()
    {
        if (isLooped)
        {
            PlayTrackAsync();
        }
        else
        {
            PlayNextTrack();
        }
    }

    public void PlayNextTrack()
    {
        if (playlist != null)
        {
            _index = (_index + 1) % playlist.Tracks.Count;
            currentTrack = playlist.Tracks[_index];
            PlayTrackAsync();
        }
    }

    public void PlayPreviousTrack()
    {
        if (playlist != null)
        {
            _index = (_index - 1 + playlist.Tracks.Count) % playlist.Tracks.Count;
            currentTrack = playlist.Tracks[_index];
            PlayTrackAsync();
        }
    }

    public void ChangeLooped() => isLooped = !isLooped;

    public async void PlayTrackAsync()
    {
        if (currentTrack != null)
        {
            await _audioService.PlayAsync(currentTrack.Link);
        }
    }

    public void StopTrack()
    {
        _audioService.Stop();
    }

    public void PauseTrack()
    {
        _audioService.Pause();
    }

}