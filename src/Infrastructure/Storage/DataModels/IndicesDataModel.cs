using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MusicService.Domain;

namespace MusicService.Infrastructure.Storage.DataModels;

public class IndicesDataModel: IIndicesDataModel
{
    private readonly string filePath;
    public string FilePath => filePath;
    

    public IndicesDataModel(string directoryPath)
    {
        filePath = Path.Combine(directoryPath, "indices.json");
    }


    public void Init()
    {
        if (!File.Exists(filePath)) 
        {
            File.WriteAllText(filePath, "0");
        }
    }


    public async Task<int> Load()
    {
        await using FileStream openStream = File.OpenRead(filePath);

        var result = await JsonSerializer.DeserializeAsync<int>(openStream);

        return result;
    }


    public async Task Save(int index)
    {
        await using FileStream createStream = File.Create(filePath);

        await JsonSerializer.SerializeAsync(createStream, index);
    }
}