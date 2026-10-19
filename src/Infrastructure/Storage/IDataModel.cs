using System.Threading.Tasks;

namespace MusicService.Infrastructure.Storage;

public interface IDataModel<TData>
{
    string FilePath { get; }
    void Init();
    Task Save(TData data);
    Task<TData> Load();
}