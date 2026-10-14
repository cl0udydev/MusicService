using System.Threading.Tasks;
using NAudio.Wave;
using System;
using MusicService.Infrastructure;

namespace MusicService.Audio;

public class AudioService
{
    private MediaFoundationReader? _mediaReader;
    private WasapiPlayer _outputDevice;
    private string? _currentTrackUrl;
    public long CurrentPosition => _mediaReader?.Position ?? 0;
    public double TotalSeconds => _mediaReader?.TotalTime.TotalSeconds ?? 0;
    public double CurrentSeconds => _mediaReader?.CurrentTime.TotalSeconds ?? 0;


    public AudioService()
    {
        _outputDevice = new WasapiPlayerBuilder().Build();
        _outputDevice.PlaybackStopped += EventBus.EmitTrackFinished;
    }

    public async Task PlayAsync(string url)
    {
        if (url == _currentTrackUrl)
        {
            if (_outputDevice.PlaybackState != PlaybackState.Playing)
            {
                _outputDevice.Play();
            }
            return;
        }

        Stop();

        _currentTrackUrl = url;

        try
        {
            _mediaReader = await Task.Run(() => new MediaFoundationReader(url));
            _outputDevice.Init(_mediaReader);
            _outputDevice.Play();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Streaming error: {ex.Message}");
            Stop();
            throw; 
        }
    }

    public void Stop()
    {
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
}