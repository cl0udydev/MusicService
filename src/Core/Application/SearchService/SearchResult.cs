using System;
using System.Collections.Generic;
using MusicService.Core.Domain;

namespace MusicService.Core.Applic.SearchService;

// The result returned by the Parser/SearchService
public readonly struct SearchResult
{
    public SearchResultType Type { get; }
    public string Message { get; }
    public List<Track>? Tracks { get; }
    public string? Lyrics { get; }
    public byte[]? CoverBytes { get; }

    public SearchResult(SearchResultType type, List<Track> tracks, string message)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException("Incoming SearchResultType must be Successfully", nameof(type));
        }
        Type = type;
        Tracks = tracks;
        Message = message;
    }

    public SearchResult(SearchResultType type, string lyrics, string message)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException("Incoming SearchResultType must be Successfully", nameof(type));
        }
        Type = type;
        Lyrics = lyrics;  
        Message = message;
    }

    public SearchResult(SearchResultType type, byte[] cover, string message)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException("Incoming SearchResultType must be Successfully", nameof(type));
        }
        Type = type;
        CoverBytes = cover;
        Message = message;
    }

    public SearchResult(SearchResultType type, string message)
    {
        if (type == SearchResultType.Successfully)
        {
            throw new ArgumentException("Incoming SearchResultType must be Failed", nameof(type));
        }
        Type = type;
        Message = message;
    }
}

public enum SearchResultType
{
    Successfully,
    Failed,
}