using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using MusicService.Sources;
using MusicService.Domain;
using System.Text;

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

            string lyricsLink = $"https://genius.com/{author.Replace(" ", "-")}-{name.Replace(" ", "-")}-lyrics";

            Track track = new Track(trackUrl, name, author, duration, trackImageLink, lyricsLink);

            tracks.Add(track);
        }

        if (tracks.Count == 0)
        {
            return new SearchResult(SearchResultType.Failed, "no tracks found");
        }

        return new SearchResult(SearchResultType.Successfully, tracks, "successfully");
    }

    public SearchResult ParseLyrics(HtmlDocument document)
    {
        HtmlNode? errorNode = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'render_404')]");

        if (errorNode != null) return new SearchResult(SearchResultType.Failed, "no lirycs was found at this link");

        HtmlNodeCollection? lyricsNode = document.DocumentNode.SelectNodes("//div[contains(@class, 'Lyrics__Container-sc-12f9e8d3-1 jhVhlB')]");

        if (lyricsNode == null) return new SearchResult(SearchResultType.Failed, "no lyrics nodes were found");

        StringBuilder lyrics = new StringBuilder();
        
        foreach (var node in lyricsNode)
        {
            foreach (var child in node.ChildNodes)
            {
                if (child.NodeType == HtmlNodeType.Text)
                {
                    lyrics.Append(child.InnerText);
                }
                else if (child.Name == "br")
                {
                    lyrics.AppendLine();
                }
                else 
                {
                    lyrics.Append(child.InnerText);
                }
            }
        }

        var cleanLyrics = HtmlEntity.DeEntitize(lyrics.ToString().Trim()).ToString();

        int index = cleanLyrics.IndexOf('[');

        if (index != -1)
            cleanLyrics = cleanLyrics[(index)..];

        return new SearchResult(SearchResultType.Successfully, cleanLyrics, "successfully");
    }
}