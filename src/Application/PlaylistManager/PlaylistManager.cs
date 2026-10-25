using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicService.Domain;
using MusicService.Infrastructure.Storage;

namespace MusicService.Applic.PlaylistManager;

public class PlaylistManager
{
    private Dictionary<int, Playlist> playlists;
    public IReadOnlyDictionary<int, Playlist> Playlists => playlists;
    private int allocatedIdLimit;
    private const int PoolCoefficient = 5;
    private Queue<int> availableIds;


    private PlaylistManager()
    {
        allocatedIdLimit = 0; // stub
        playlists = new(); // stub
        availableIds = new(); // stub
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
                availableIds.Enqueue(index);
            }
        }
    }


    public async Task<Playlist> CreatePlaylist()
    {
        var index = await TakeIndex();

        Playlist newPlaylist = new Playlist(index);
        playlists.Add(index, newPlaylist);

        await AppDataService.Playlists.Save(playlists);

        return newPlaylist;
    }


    public async Task DeletePlaylist(int id)
    {
        if (!playlists.Remove(id))
            throw new KeyNotFoundException($"Playlist with ID {id} was not found.");

        
        availableIds.Enqueue(id);

        await AppDataService.Playlists.Save(playlists);
    }


    public async Task AddTrackToPlaylist(int id, Track track)
    {
        if (!playlists.ContainsKey(id))
            throw new KeyNotFoundException($"Playlist with ID {id} was not found.");
        
        playlists[id].AddTrack(track);

        await AppDataService.Playlists.Save(playlists);
    }

    
    public async Task DeleteTrackFromPlaylist(int id, Track track)
    {
        if (!playlists.ContainsKey(id))
            throw new KeyNotFoundException($"Playlist with ID {id} was not found.");
        
        playlists[id].RemoveTrack(track);

        await AppDataService.Playlists.Save(playlists);
    }


    private async Task<int> TakeIndex()
    {
        if (availableIds.Count == 0)
        {
            await ExpandPool();
        }

        var index = availableIds.Dequeue();
        
        return index;
    }


    private async Task ExpandPool()
    {
        var range = Enumerable.Range(allocatedIdLimit + 1, PoolCoefficient);

        allocatedIdLimit += PoolCoefficient;

        await AppDataService.Indices.Save(allocatedIdLimit);

        foreach (var i in range)
        {
            availableIds.Enqueue(i);
        }
    }
}