namespace MusicService.Domain;

public interface IPlaylistOwnable
{
    Playlist? Playlist { get; }
    

    void ChangePlaylist(Playlist newPlaylist, int index);
}