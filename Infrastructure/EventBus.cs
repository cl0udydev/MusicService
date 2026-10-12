using System;
using NAudio.Wave;

namespace MusicService.Infrastructure;

internal sealed class EventBus
{
    private EventBus(){}

    public static event Action<StoppedEventArgs> TrackFinished;

    public static void EmitTrackFinished(StoppedEventArgs args)
    {
        TrackFinished?.Invoke(args);
    }
}