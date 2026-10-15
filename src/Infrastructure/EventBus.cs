using System;
using NAudio.Wave;

namespace MusicService.Infrastructure;

internal static class EventBus
{
    public static event Action<StoppedEventArgs>? TrackFinished;


    public static void EmitTrackFinished(object? sender, StoppedEventArgs args)
    {
        TrackFinished?.Invoke(args);
    }
}