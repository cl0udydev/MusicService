using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace MusicService.Core.Infrastructure;

public static class ConfigurationSystem
{
    private static string APIKEYFilePath = @$"{Directory.GetCurrentDirectory()}\Settings\API_SETTINGS.json";
    private static string APIKEYDirectory = @$"{Directory.GetCurrentDirectory()}\Settings";
    private static Dictionary<API_CONFIGURATION_TYPE, string?>? API_Tokens;

    public static string? GetToken(API_CONFIGURATION_TYPE type)
    {
        return API_Tokens?[type];
    }

    public static void Init()
    {
        if (!File.Exists(APIKEYFilePath))
        {
            Directory.CreateDirectory(APIKEYDirectory);
            File.WriteAllText(APIKEYFilePath, "{}");

            using FileStream openStream = File.Create(APIKEYFilePath);
            var tempDictionary = new Dictionary<API_CONFIGURATION_TYPE, string?>()
            {
                {API_CONFIGURATION_TYPE.AUDIUS_API_TOKEN, null},
                {API_CONFIGURATION_TYPE.AUDIUS_API_BEARER_TOKEN, null},
                {API_CONFIGURATION_TYPE.JAMENDO_API_TOKEN, null},
            };

            JsonSerializer.Serialize(
                openStream, tempDictionary, new JsonSerializerOptions{WriteIndented = true}
            );
            API_Tokens = new(tempDictionary);
        }
        else
        {
            using FileStream openStream = File.OpenRead(APIKEYFilePath);
            API_Tokens = JsonSerializer.Deserialize<Dictionary<API_CONFIGURATION_TYPE, string?>>(openStream);
        }
        
    }
}

public enum API_CONFIGURATION_TYPE
{
    AUDIUS_API_TOKEN,
    AUDIUS_API_BEARER_TOKEN,
    JAMENDO_API_TOKEN,
} 