using System.Threading.Tasks;
using System.Collections.Generic;
using MusicService.Core.Domain;

namespace MusicService.Core.Infrastructure.Storage.DataModels;

public interface IDataModel<TData>
{
    string FilePath { get; }
    void Init();
    Task Save(TData data);
    Task<TData> Load();
}

public interface IIndicesDataModel: IDataModel<int> {}
public interface IPlaylistsDataModel: IDataModel<Dictionary<int, Playlist>> {}