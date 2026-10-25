using System;
using System.Collections.Generic;

namespace MusicService.Domain;

// The result returned by the Parser
public readonly struct SearchResult
{
    public SearchResultType Type { get; }
    public List<Track>? Tracks { get; }

    public SearchResult(SearchResultType type, List<Track> tracks)
    {
        if (type != SearchResultType.Successfully)
        {
            throw new ArgumentException("here incoming SearchResultType must be Successfully", nameof(type));
        }
        Type = type;
        Tracks = tracks;
    }

    public SearchResult(SearchResultType type)
    {
        if (type == SearchResultType.Successfully)
        {
            throw new ArgumentException("here incoming SearchResultType must be Failed", nameof(type));
        }
        Type = type;
    }
}

public enum SearchResultType
{
    Successfully,
    Failed,
}