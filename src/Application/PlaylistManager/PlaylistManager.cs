using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MusicService.Domain;
using MusicService.Infrastructure.Storage;

namespace MusicService.Applic.PlaylistManager;

public class PlaylistManager
{
    private Dictionary<int, Playlist> playlists;
    private int maxFreeId;
    private const int pullCoef = 5;
    private Queue<int> indicesPull;


    public PlaylistManager()
    {
        maxFreeId = 0; // stub
        playlists = new(); // stub
        indicesPull = new(); // stub
    }

    public async Task InitAsync()
    {
        maxFreeId = await AppDataService.Indices.Load();
        playlists = await AppDataService.Playlists.Load();

        foreach (var index in Enumerable.Range(1, maxFreeId))
        {
            if (!playlists.ContainsKey(index))
            {
                indicesPull.Enqueue(index);
            }
        }
    }


    public async Task CreatePlaylist()
    {
        var index = TakeIndex();

        await AppDataService.Indices.Save(maxFreeId);

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


    private int TakeIndex()
    {
        if (indicesPull.Count == 0)
        {
            ExpandPull();
        }

        var index = indicesPull.Dequeue();
        
        return index;
    }


    private void ExpandPull()
    {
        var range = Enumerable.Range(maxFreeId + 1, pullCoef);

        foreach (var i in range)
        {
            indicesPull.Enqueue(i);
        }

        maxFreeId += pullCoef;

    }
}