using System;
using System.IO;

namespace MusicService.Infrastructure.Storage;

internal static class AppDataService
{
    private static string appDataDirectory { get; } = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.LocalApplicationData), "MusicService");

    public static string LibraryFile => Path.Combine(appDataDirectory, "data.json");


    public static void InitAppData()
    {
        Directory.CreateDirectory(appDataDirectory);
    }
}