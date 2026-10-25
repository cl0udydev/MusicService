using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicService.Domain;
using MusicService.Infrastructure.Storage;

namespace MusicService.Applic.PlaylistManager;

public class PlaylistManager
{
    private Dictionary<int, Playlist> playlists;
    private int allocatedIdLimit;
    private const int PoolCoefficient = 5;
    private Queue<int> indicesPull;


    private PlaylistManager()
    {
        allocatedIdLimit = 0; // stub
        playlists = new(); // stub
        indicesPull = new(); // stub
    }

    public static async Task<PlaylistManager> CreateAsync()
    {
        var manager = new PlaylistManager();

        await manager.InitAsync();

        return manager;
    }

    private async Task InitAsync()
    {
        allocatedIdLimit = await AppDataService.Indices.Load();
        playlists = await AppDataService.Playlists.Load();

        foreach (var index in Enumerable.Range(1, allocatedIdLimit))
        {
            if (!playlists.ContainsKey(index))
            {
                indicesPull.Enqueue(index);
            }
        }
    }


    public async Task CreatePlaylist()
    {
        var index = await TakeIndex();

        Playlist newPLaylist = new Playlist(index);
        playlists.Add(index, newPLaylist);

        await AppDataService.Playlists.Save(playlists);
    }


    public async Task DeletePlayList(int id)
    {
        if (!playlists.Remove(id))
        {
            throw new KeyNotFoundException($"Playlist with ID {id} was not found.");
        }
        
        indicesPull.Enqueue(id);

        await AppDataService.Playlists.Save(playlists);
    }


    private async Task<int> TakeIndex()
    {
        if (indicesPull.Count == 0)
        {
            await ExpandPull();
        }

        var index = indicesPull.Dequeue();
        
        return index;
    }


    private async Task ExpandPull()
    {
        var range = Enumerable.Range(allocatedIdLimit + 1, PoolCoefficient);

        foreach (var i in range)
        {
            indicesPull.Enqueue(i);
        }

        allocatedIdLimit += PoolCoefficient;

        await AppDataService.Indices.Save(allocatedIdLimit);
    }
}