using System;
using System.Collections.Generic;

namespace MusicService.Infrastructure;

internal static class NetworkErrorsSystem
{
    public static event Action<Exception>? SearchErrorTriggered;
    public static event Action<Exception>? PlaybackErrorTriggered;
    public static event Action<Exception>? ResourceLoadingTriggered;
    private static readonly Dictionary<NetworkErrorType, Action<Exception>> ErrorHandlers = new()
    {        
        { NetworkErrorType.PlaybackError, ex => PlaybackErrorTriggered?.Invoke(ex) },
        { NetworkErrorType.SearchError, ex => SearchErrorTriggered?.Invoke(ex) },
        { NetworkErrorType.LoadingError, ex => ResourceLoadingTriggered?.Invoke(ex) }
    };

    public static void EmitNetworkError(Exception exception, NetworkErrorType type)
    {
        ErrorHandlers[type](exception);
    }

}

public enum NetworkErrorType
{
    SearchError, PlaybackError, LoadingError,
}