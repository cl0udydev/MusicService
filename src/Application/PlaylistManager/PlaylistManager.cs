using System;
using System.Collections.Generic;
using MusicService.Domain;

namespace MusicService.Applic.PlaylistManager;

public class PlaylistManager
{
    private Dictionary<int, Playlist> playlists;
    private int nextFreeId;

    public PlaylistManager()
    {
        nextFreeId = 0;
        playlists = new();
    }
    public void CreatePlaylist()
    {
        Playlist newPLaylist = new Playlist(nextFreeId);
        nextFreeId++;
    }

    public void DeletePlayList(int id)
    {
        if (!playlists.Remove(id))
        {
            throw new KeyNotFoundException($"Playlist with ID {id} was not found.");
        }
    }

    // TODO: need to add methods like Add/Remove track, solve the problem with indices
}