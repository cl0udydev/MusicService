using System.Threading.Tasks;
using NAudio.Wave;
using System;
using MusicService.Core.Infrastructure;
using MusicService.Core.Applic.PlaybackController;

namespace MusicService.Core.Audio;

public class AudioService
{
    private MediaFoundationReader? _mediaReader;
    private WasapiPlayer? _outputDevice;
    private string? _currentTrackUrl;
    private PlaybackStopReason? _requestedStopReason;
    public event Action? TrackFinished;
    public event Action<PlaybackState>? PlaybackStateChanged;
    private float volume = 0.5f * 0.4f;
    public long CurrentPosition => _mediaReader?.Position ?? 0;
    public double TotalSeconds => _mediaReader?.TotalTime.TotalSeconds ?? 0;
    public double CurrentSeconds => _mediaReader?.CurrentTime.TotalSeconds ?? 0;


    public void ResetUrl() => _currentTrackUrl = null; 

    private void OnPlaybackStopped(object? sender, StoppedEventArgs args)
    {
        if (!ReferenceEquals(sender, _outputDevice)) return;
        if (args.Exception != null)
        {
            DisposeAll();
            PlaybackStateChanged?.Invoke(PlaybackState.Stopped);
            NetworkErrorsSystem.EmitNetworkError(args.Exception, NetworkErrorType.PlaybackError);
            return;
        }

        if (_requestedStopReason != null)
        {
            _requestedStopReason = null;
            return;
        }

        PlaybackStateChanged?.Invoke(PlaybackState.Stopped);
        TrackFinished?.Invoke();
    }


    public async Task PlayAsync(string url)
    {
        if (url == _currentTrackUrl && _mediaReader != null)
        {
            if (_outputDevice != null && _outputDevice.PlaybackState != PlaybackState.Playing)
            {
                if (_mediaReader.CurrentTime >= _mediaReader.TotalTime)
                {
                    _mediaReader.CurrentTime = TimeSpan.Zero;
                }

                _outputDevice.Play();
                PlaybackStateChanged?.Invoke(PlaybackState.Playing);
            }

            return;
        }

        Stop(PlaybackStopReason.TrackSwitch);
        RecreateOutputDevice();

        try
        {
            _mediaReader = await Task.Run(() => new MediaFoundationReader(url));
            _currentTrackUrl = url;
            _outputDevice!.Init(_mediaReader);
            _outputDevice.Volume = volume;
            _outputDevice.Play();
            PlaybackStateChanged?.Invoke(PlaybackState.Playing);
        }
        catch (Exception ex)
        {
            DisposeAll();
            PlaybackStateChanged?.Invoke(PlaybackState.Stopped);
            NetworkErrorsSystem.EmitNetworkError(ex, NetworkErrorType.LoadingError);
        }
    }


    public void Stop(PlaybackStopReason reason = PlaybackStopReason.UserStopped)
    {
        if (_outputDevice == null) return;
        if (_outputDevice.PlaybackState != PlaybackState.Stopped) _requestedStopReason = reason;

        _outputDevice.Stop();

        _mediaReader?.Dispose();
        _mediaReader = null;
        _currentTrackUrl = null;

        PlaybackStateChanged?.Invoke(PlaybackState.Stopped);
    }


    public void Pause()
    {
        if (_outputDevice == null) return;
        if (_outputDevice.PlaybackState == PlaybackState.Playing)
        {
            _outputDevice.Pause();
            PlaybackStateChanged?.Invoke(PlaybackState.Paused);
        }
    }


    public void SetVolume(float volume)
    {
        this.volume = volume * 0.4f;

        if (_outputDevice != null) _outputDevice.Volume = this.volume;
    }


    public void Seek(double targetSeconds)
    {
        if (_mediaReader != null)
        {
            double safeSeconds = Math.Clamp(targetSeconds, 0, TotalSeconds);
            _mediaReader.CurrentTime = TimeSpan.FromSeconds(safeSeconds);
        }
    }

    private void RecreateOutputDevice()
    {
        if (_outputDevice != null)
        {
            var oldDevice = _outputDevice;
            _outputDevice = null;

            oldDevice.PlaybackStopped -= OnPlaybackStopped;
            oldDevice.Dispose();
        }

        _requestedStopReason = null;

        _outputDevice = new WasapiPlayerBuilder().Build();
        _outputDevice.PlaybackStopped += OnPlaybackStopped;
    }


    private void DisposeAll()
    {
        _outputDevice?.Stop();
        _outputDevice?.PlaybackStopped -= OnPlaybackStopped;
        _outputDevice?.Dispose();
        _outputDevice = null;
        _mediaReader?.Dispose();
        _mediaReader = null;
        _currentTrackUrl = null;
    }
}