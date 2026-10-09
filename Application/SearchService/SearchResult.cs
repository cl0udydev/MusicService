using System;
using System.Collections.Generic;
using MusicService.Domain;

namespace MusicService.App.SearchService;

// The result returned by the Parser
public readonly struct SearchResult
{
    public SearchResultType Type { get; }
    public List<Track>? Tracks { get; }

    public SearchResult(SearchResultType type, List<Track> tracks)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException($"Incoming SearchResultType: {type}, at incoming tracks it must be Successfully");
        }
        Type = type;
        Tracks = tracks;
    }

    public SearchResult(SearchResultType type)
    {
        if (type == SearchResultType.Successfully)
        {
            throw new ArgumentException($"Incoming SearchResultType: {type}, in the absence tracks it must be different");
        }
        Type = type;
    }
}

public enum SearchResultType
{
    Successfully,
    Failed,
}