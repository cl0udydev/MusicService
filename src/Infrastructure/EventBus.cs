using System;
using NAudio.Wave;

namespace MusicService.Infrastructure;

internal static class EventBus
{
    public static event Action? TrackFinished;


    public static void EmitTrackFinished()
    {
        TrackFinished?.Invoke();
    }
}