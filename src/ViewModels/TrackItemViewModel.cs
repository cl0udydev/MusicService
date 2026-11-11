using CommunityToolkit.Mvvm.ComponentModel;
using MusicService.Core.Domain;
using Avalonia.Media.Imaging;

namespace MusicService.ViewModels;

public sealed class TrackItemViewModel : ObservableObject
{
    public Track Track { get; }
    public string Name => Track.Name;
    public string Author => Track.Author;
    public string Duration => Track.Duration.ToString(@"m\:ss");

    private Bitmap? _coverImage;
    public Bitmap? CoverImage
    {
        get => _coverImage;
        private set => SetProperty(ref _coverImage, value);
    }

    public TrackItemViewModel(Track track, Bitmap? cover)
    {
        Track = track;
        CoverImage = cover;
    }
}