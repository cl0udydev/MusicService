using System;
using System.IO;
using MusicService.Infrastructure.Storage.DataModels;

namespace MusicService.Infrastructure.Storage;

internal static class AppDataService
{
    private static string appDataDirectory { get; } = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData), "MusicService");

    private static IPlaylistsDataModel playlistsModel = new PlaylistsDataModel(appDataDirectory);
    private static IIndicesDataModel indicesModel = new IndicesDataModel(appDataDirectory);
    public static IPlaylistsDataModel Playlists => playlistsModel;
    public static IIndicesDataModel Indices => indicesModel;


    public static void InitAppData()
    {
        Directory.CreateDirectory(appDataDirectory);
        playlistsModel.Init();
        indicesModel.Init();
    }
}
