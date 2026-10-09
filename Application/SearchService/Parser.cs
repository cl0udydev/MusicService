using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Sources;
using MusicService.Domain;

namespace MusicService.Applic.SearchService;

// parser html document to SearchResult => SearchService => ViewModel
public class Parser
{
    public SearchResult ParseSource(HtmlDocument document, Dictionary<TrackSourceType, string> parseRules)
    {

        HtmlNodeCollection? trackNodes = document.DocumentNode.SelectNodes(
            $"//li[contains(@class, '{parseRules[TrackSourceType.General]}')]"
        );

        if (trackNodes == null) return new SearchResult(SearchResultType.Failed);

        List<Track> tracks = new();

        foreach (HtmlNode trackNode in trackNodes)
        {
            HtmlNode trackName = trackNode.SelectSingleNode($".//div[contains(@class, '{parseRules[TrackSourceType.Name]}')]")!;
            HtmlNode trackAuthor = trackNode.SelectSingleNode($".//div[contains(@class, '{parseRules[TrackSourceType.Author]}')]")!;
            HtmlNode trackDuration = trackNode.SelectSingleNode($".//div[contains(@class, '{parseRules[TrackSourceType.Duration]}')]")!;
            HtmlNode UrlNode = trackNode.SelectSingleNode($".//div[contains(@class, '{parseRules[TrackSourceType.Link]}')]")!;

            string trackUrl = UrlNode.GetAttributeValue("href", "not found");
            string name = trackName.InnerText.Trim();
            string author = trackAuthor.InnerText.Trim();
            string durationStr = trackDuration.InnerText.Trim();

            TimeSpan duration;

            if (!TimeSpan.TryParse(durationStr, out duration))
            {
                if (TimeSpan.TryParseExact(durationStr, @"mm\:ss", null, out duration)) {}
                else if (TimeSpan.TryParseExact(durationStr, @"m\:ss", null, out duration)) {}
            }

            Track track = new Track(trackUrl, name, author, duration);

            tracks.Add(track);
        }

        if (tracks.Count == 0)
        {
            return new SearchResult(SearchResultType.Failed);
        }

        return new SearchResult(SearchResultType.Successfully, tracks);
    }
}