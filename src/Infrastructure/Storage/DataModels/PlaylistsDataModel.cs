using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MusicService.Domain;

namespace MusicService.Infrastructure.Storage.DataModels;

public class PlaylistsDataModel: IPlaylistsDataModel
{
    private string filePath;
    public string FilePath => filePath;
    

    public PlaylistsDataModel(string directoryPath)
    {
        filePath = Path.Combine(directoryPath, "playlists.json");
    }


    public void Init()
    {
        if (!File.Exists(filePath)) 
        {
            File.WriteAllText(filePath, "{}");
        }
    }


    public async Task<Dictionary<int, Playlist>> Load()
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true
        };

        await using FileStream openStream = File.OpenRead(filePath);

        var result = await JsonSerializer.DeserializeAsync<Dictionary<int, Playlist>>(openStream, options);

        return result ?? new Dictionary<int, Playlist>();
    }


    public async Task Save(Dictionary<int, Playlist> playlists)
    {
        var options = new JsonSerializerOptions
        {
            IncludeFields = true, // force read private readonly field in playlists
            WriteIndented = true  // make json readable for people
        };

        await using FileStream createStream = File.Create(filePath);

        await JsonSerializer.SerializeAsync(createStream, playlists, options);
    }
}