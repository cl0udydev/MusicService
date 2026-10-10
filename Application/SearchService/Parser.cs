using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Sources;
using MusicService.Domain;

namespace MusicService.Applic.SearchService;

// parser html document to SearchResult => SearchService => ViewModel
public class Parser
{
    public SearchResult ParseSource(HtmlDocument document, Dictionary<TrackSourceType, TrackSourceParseRule> parseRules)
    {
        // take html class and tag from parseRules and inserts them to SelectNodes args
        var htmlTag = parseRules[TrackSourceType.General].HtmlTag;
        var htmlClass = parseRules[TrackSourceType.General].HtmlClass;

        HtmlNodeCollection? trackNodes = document.DocumentNode.SelectNodes(
            $"//{htmlTag}[contains(@class, '{htmlClass}')]"
        );

        if (trackNodes == null) return new SearchResult(SearchResultType.Failed);

        List<Track> tracks = new();

        foreach (HtmlNode trackNode in trackNodes)
        {
            HtmlNode trackName = trackNode.SelectSingleNode(
                $".//{parseRules[TrackSourceType.Name].HtmlTag}[contains(@class, '{parseRules[TrackSourceType.Name].HtmlClass}')]"
            )!;

            HtmlNode trackAuthor = trackNode.SelectSingleNode(
                $".//{parseRules[TrackSourceType.Author].HtmlTag}[contains(@class, '{parseRules[TrackSourceType.Author].HtmlClass}')]"
            )!;

            HtmlNode trackDuration = trackNode.SelectSingleNode(
                $".//{parseRules[TrackSourceType.Duration].HtmlTag}[contains(@class, '{parseRules[TrackSourceType.Duration].HtmlClass}')]"
            )!;

            HtmlNode UrlNode = trackNode.SelectSingleNode(
                $".//{parseRules[TrackSourceType.Link].HtmlTag}[contains(@class, '{parseRules[TrackSourceType.Link].HtmlClass}')]"
            )!;

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