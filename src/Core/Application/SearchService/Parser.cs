using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Core.Sources;
using MusicService.Core.Domain;
using System.Text.Json;

namespace MusicService.Core.Applic.SearchService;

// parser html document to SearchResult => SearchService => ViewModel
public class Parser
{
    public SearchResult ParseSource(JsonDocument document, IMusicSource musicSource)
    {
        var tracksListJson = musicSource.GetTracksList(document).EnumerateArray();
        var tracksList = new List<Track>();

        foreach (var trackElement in tracksListJson)
        {
            var trackName = musicSource.GetName(trackElement);
            var trackAuthor = musicSource.GetAuthor(trackElement);
            var trackDuration = musicSource.GetDuration(trackElement);
            var trackLink = musicSource.GetLink(trackElement);
            var trackImageLink = musicSource.GetImageLink(trackElement);

            Console.WriteLine(trackName, trackAuthor, trackDuration, trackLink, trackImageLink);
            string escapedArtist = trackAuthor!.Replace(" ", "+");
            string escapedTitle = trackName!.Replace(" ", "+");

            var trackLyricsLink = $"https://lrclib.net/api/get?artist_name={escapedArtist}&track_name={escapedTitle}";

            tracksList.Add(new Track(trackLink, trackName, trackAuthor, trackDuration, trackImageLink, trackLyricsLink));
        }
        if (tracksList.Count == 0) return new SearchResult(SearchResultType.Failed, "tracks list equal to 0");
        return new SearchResult(SearchResultType.Successfully, tracksList, "successfuly");     
    }

    public SearchResult ParseLyrics(JsonDocument document)
    {
        string lyrics = "";
        
        if (document.RootElement.TryGetProperty("plainLyrics", out var plainLyricsElement))
        {
            lyrics = plainLyricsElement.GetString() ?? "";
        }

        if (string.IsNullOrWhiteSpace(lyrics) && document.RootElement.TryGetProperty("syncedLyrics", out var syncedElement))
        {
            lyrics = syncedElement.GetString() ?? "";
        }

        if (string.IsNullOrWhiteSpace(lyrics))
        {
            return new SearchResult(SearchResultType.Failed, "track lyrics not found");
        }

        return new SearchResult(SearchResultType.Successfully, lyrics.Trim(), "successfully");
    }

}