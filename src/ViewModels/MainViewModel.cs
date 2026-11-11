using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicService.Core.Applic.PlaybackController;
using MusicService.Core.Applic.PlaylistManager;
using MusicService.Core.Applic.SearchService;
using MusicService.Core.Audio;
using MusicService.Core.Domain;
using MusicService.Core.Infrastructure.Storage;
using MusicService.Core.Sources;

namespace MusicService.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private PlaylistManager playlistManager;
    private PlaybackController playbackController;
    private AudioService audioService;
    private SearchService searchService;

    public MainViewModel()
    {
        searchService = new SearchService(new HitmoMusicSource());
        audioService = new AudioService();
        playbackController = new PlaybackController(audioService);
    }
    
    
    public async Task InitializeAsync()
    {
        AppDataService.InitAppData();
        playlistManager = await PlaylistManager.CreateAsync();
    }

}
