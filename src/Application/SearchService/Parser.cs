using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Sources;
using MusicService.Domain;
using System.Text;
using System.Text.Json;

namespace MusicService.Applic.SearchService;

// parser html document to SearchResult => SearchService => ViewModel
public class Parser
{
    public SearchResult ParseSource(HtmlDocument document, IMusicSource musicSource)
    {
        HtmlNodeCollection? trackNodes = document.DocumentNode.SelectNodes(musicSource.GeneralNodeArgs);

        if (trackNodes == null) return new SearchResult(SearchResultType.Failed, "main nodes of the track were not found");

        List<Track> tracks = new();

        foreach (HtmlNode trackNode in trackNodes)
        {
            HtmlNode? trackName = musicSource.GetNameNode(trackNode);

            HtmlNode? trackAuthor = musicSource.GetAuthorNode(trackNode);

            HtmlNode? trackDuration = musicSource.GetDurationNode(trackNode);

            string? trackUrl = musicSource.GetLink(trackNode);

            string? trackImageLink = musicSource.GetImageLink(trackNode);
            
            if (trackName == null || trackAuthor == null || trackDuration == null || trackUrl == null || trackImageLink == null)
            {
                continue; 
            }

            string name = trackName.InnerText.Trim();
            string author = trackAuthor.InnerText.Trim();

            TimeSpan duration = TimeSpan.Zero;

            string durationStr = trackDuration.InnerText.Trim(); 

            if (TimeSpan.TryParseExact(durationStr, @"mm\:ss", null, out duration)) { }
            else if (TimeSpan.TryParseExact(durationStr, @"m\:ss", null, out duration)) { }
            else if (TimeSpan.TryParseExact(durationStr, @"h\:mm\:ss", null, out duration)) { }
            else if (TimeSpan.TryParse(durationStr, out duration)) { }

            string escapedArtist = author.Replace(" ", "+");
            string escapedTitle = name.Replace(" ", "+");

            string lyricsLink = $"https://lrclib.net/api/get?artist_name={escapedArtist}&track_name={escapedTitle}";

            Track track = new Track(trackUrl, name, author, duration, trackImageLink, lyricsLink);

            tracks.Add(track);
        }

        if (tracks.Count == 0)
        {
            return new SearchResult(SearchResultType.Failed, "no tracks found");
        }

        return new SearchResult(SearchResultType.Successfully, tracks, "successfully");
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