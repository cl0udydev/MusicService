using System.Threading.Tasks;
using NAudio.Wave;

namespace MusicService.Audio;

public class AudioService
{
    private MediaFoundationReader? _mediaReader;
    private IWavePlayer _outputDevice;
    private string? _currentTrackUrl;

    public AudioService()
    {
        _outputDevice = new WasapiPlayerBuilder().Build();
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

        _mediaReader = await Task.Run(() => new MediaFoundationReader(url));
        _outputDevice.Init(_mediaReader);
        _outputDevice.Play();
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


}