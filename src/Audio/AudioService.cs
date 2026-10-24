using System.Threading.Tasks;
using NAudio.Wave;
using System;
using MusicService.Infrastructure;
using MusicService.Domain;

namespace MusicService.Audio;

public class AudioService
{
    private MediaFoundationReader? _mediaReader;
    private WasapiPlayer _outputDevice;
    private string? _currentTrackUrl;
    private PlaybackStopReason? _requestedStopReason;
    public long CurrentPosition => _mediaReader?.Position ?? 0;
    public double TotalSeconds => _mediaReader?.TotalTime.TotalSeconds ?? 0;
    public double CurrentSeconds => _mediaReader?.CurrentTime.TotalSeconds ?? 0;


    public AudioService()
    {
        _outputDevice = new WasapiPlayerBuilder().Build();
        _outputDevice.PlaybackStopped += OnPlaybackStopped;
    }


    private void OnPlaybackStopped(object? sender, StoppedEventArgs args)
    {
        if (args.Exception != null)
        {
            NetworkErrorsSystem.EmitNetworkError(args.Exception, NetworkErrorType.PlaybackError);
            return;
        }

        if (_requestedStopReason != null)
        {
            _requestedStopReason = null;
            return;
        }

        EventBus.EmitTrackFinished();
    }


    public async Task PlayAsync(string url)
    {
        if (url == _currentTrackUrl)
        {
            if (_outputDevice.PlaybackState != PlaybackState.Playing)
                _outputDevice.Play();

            return;
        }

        Stop(PlaybackStopReason.TrackSwitch);
        _currentTrackUrl = url;

        try
        {
            _mediaReader = await Task.Run(() => new MediaFoundationReader(url));
            _outputDevice.Init(_mediaReader);
            _outputDevice.Play();
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, NetworkErrorType.LoadingError);
        }
    }


    public void Stop(PlaybackStopReason reason = PlaybackStopReason.UserStopped)
    {
        if (_outputDevice.PlaybackState != PlaybackState.Stopped) _requestedStopReason = reason;
        _outputDevice.Stop();

        _mediaReader?.Dispose();
        _mediaReader = null;
        _currentTrackUrl = null;
    }


    public void Pause()
    {
        if (_outputDevice.PlaybackState == PlaybackState.Playing)
        {
            _outputDevice.Pause();
        }
    }


    public void SetVolume(float volume)
    {
        _outputDevice.Volume = volume;
    }


    public void Seek(double targetSeconds)
    {
        if (_mediaReader != null)
        {
            double safeSeconds = Math.Clamp(targetSeconds, 0, TotalSeconds);
            _mediaReader.CurrentTime = TimeSpan.FromSeconds(safeSeconds);
        }
    }


    private void CreateOutputDevice()
    {
        if (_outputDevice != null) return;
        
        _outputDevice = new WasapiPlayerBuilder().Build();
        _outputDevice.PlaybackStopped += OnPlaybackStopped;
    }

    
    private void DeleteOutputDevice()
    {
        if (_outputDevice == null) return;

        _outputDevice.PlaybackStopped -= OnPlaybackStopped;
        _outputDevice.Dispose();
    }
}