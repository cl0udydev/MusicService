using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Sources;
using MusicService.Domain;

namespace MusicService.Applic.SearchService;

// parser html document to SearchResult => SearchService => ViewModel
public class Parser
{
    public SearchResult ParseSource(HtmlDocument document, IMusicSource musicSource)
    {
        HtmlNodeCollection? trackNodes = document.DocumentNode.SelectNodes(musicSource.GeneralNodeArgs);

        if (trackNodes == null) return new SearchResult(SearchResultType.Failed);

        List<Track> tracks = new();

        foreach (HtmlNode trackNode in trackNodes)
        {
            HtmlNode? trackName = musicSource.GetNameNode(trackNode);

            HtmlNode? trackAuthor = musicSource.GetAuthorNode(trackNode);

            HtmlNode? trackDuration = musicSource.GetDurationNode(trackNode);

            string? trackUrl = musicSource.GetLinkNode(trackNode);

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


            Track track = new Track(trackUrl, name, author, duration, trackImageLink);

            tracks.Add(track);
        }

        if (tracks.Count == 0)
        {
            return new SearchResult(SearchResultType.Failed);
        }

        return new SearchResult(SearchResultType.Successfully, tracks);
    }
}