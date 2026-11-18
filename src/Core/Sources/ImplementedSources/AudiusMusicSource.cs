using System;
using System.Collections.Generic;
using System.Text.Json;
using MusicService.Core.Infrastructure;

namespace MusicService.Core.Sources;

public class AudiusMusicSource : IMusicSource
{
    public IReadOnlyDictionary<string, string?> RequestHeaders { get; }

    public AudiusMusicSource()
    {
        RequestHeaders = new Dictionary<string, string?>
        {
            { "x-api-key", ConfigurationSystem.GetToken(API_CONFIGURATION_TYPE.AUDIUS_API_TOKEN) },
            { "Authorization", ConfigurationSystem.GetToken(API_CONFIGURATION_TYPE.AUDIUS_API_BEARER_TOKEN) }
        };
    }

    public string? ValidateConfiguration()
    {
        if (string.IsNullOrWhiteSpace(
            ConfigurationSystem.GetToken(API_CONFIGURATION_TYPE.AUDIUS_API_TOKEN)))
        {
            return "Audius API Token is missing. Please check the API_SETTINGS.json file";
        }

        return null;
    }

    public string FormingLink(string userInput) =>
    $"https://discoveryprovider.audius.co/v1/tracks/search?query={Uri.EscapeDataString(userInput)}";

    public JsonElement GetTracksList(JsonDocument json) => json.RootElement.GetProperty("data"); 

    public string GetName(JsonElement trackNode) => trackNode.GetProperty("title").GetString()!;

    public string GetAuthor(JsonElement trackNode) => trackNode.GetProperty("user").GetProperty("name").GetString()!; 

    public TimeSpan GetDuration(JsonElement trackNode) => TimeSpan.FromSeconds(trackNode.GetProperty("duration").GetDouble());

    public string GetLink(JsonElement trackNode)
    {
        var id = trackNode.GetProperty("id").GetString()!;

        return $"https://discoveryprovider.audius.co/v1/tracks/{id}/stream";
    }
    
    public string GetImageLink(JsonElement trackNode)
    {
        JsonElement artwork = trackNode.GetProperty("artwork");

        return artwork.GetProperty("480x480").GetString()!;
    }

}