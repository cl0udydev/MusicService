using CommunityToolkit.Mvvm.ComponentModel;
using MusicService.Applic.PlaybackController;
using MusicService.Applic.PlaylistManager;
using MusicService.Applic.SearchService;
using MusicService.Audio;
using MusicService.Domain;
using MusicService.Infrastructure.Storage;

namespace MusicService.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private PlaylistManager playlistManager;
    private PlaybackController playbackController;
    private AudioService audioService;
    private SearchService searchService;

    
    private string currentTrackName => playbackController.CurrentTrack?.Name ?? "None";
    private string currentTrackAuthor => playbackController.CurrentTrack?.Author ?? "None";
    

    public MainViewModel()
    {
        audioService = new AudioService();
        playbackController = new PlaybackController(audioService);

        playbackController.CurrentTrackChanged += OnCurrentTrackChanged;
    }


    private void OnCurrentTrackChanged()
    {
        OnPropertyChanged(nameof(currentTrackName));
        OnPropertyChanged(nameof(currentTrackAuthor));
    }
}
