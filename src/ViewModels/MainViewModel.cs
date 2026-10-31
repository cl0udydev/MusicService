using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MusicService.Applic.PlaybackController;
using MusicService.Applic.PlaylistManager;
using MusicService.Applic.SearchService;
using MusicService.Audio;
using MusicService.Domain;
using MusicService.Infrastructure.Storage;
using MusicService.Sources;

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
