using System;
using System.Collections.Generic;
using System.Text.Json;

namespace MusicService.Core.Sources;

public interface IMusicSource
{
    IReadOnlyDictionary<string, string?> RequestHeaders { get; }
    public string? ValidateConfiguration();
    string FormingLink(string userInput);
    JsonElement GetTracksList(JsonDocument json);
    string GetName(JsonElement trackNode);
    string GetAuthor(JsonElement trackNode);
    TimeSpan GetDuration(JsonElement trackNode);
    string GetLink(JsonElement trackNode);
    string GetImageLink(JsonElement trackNode);

}