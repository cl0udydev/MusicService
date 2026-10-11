using System;
using System.Collections.Generic;
using MusicService.Domain;

namespace MusicService.Applic.SearchService;

// The result returned by the Parser
public readonly struct SearchResult
{
    public SearchResultType Type { get; }
    public List<Track>? Tracks { get; }

    public SearchResult(SearchResultType type, List<Track> tracks)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException(nameof(type), "here incoming SearchResultType must be Successfully");
        }
        Type = type;
        Tracks = tracks;
    }

    public SearchResult(SearchResultType type)
    {
        if (type == SearchResultType.Successfully)
        {
            throw new ArgumentException(nameof(type), "here incoming SearchResultType must be Failed");
        }
        Type = type;
    }
}

public enum SearchResultType
{
    Successfully,
    Failed,
}