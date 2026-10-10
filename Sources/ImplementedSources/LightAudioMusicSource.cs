namespace MusicService.Sources;

public class LightAudioMusicSource: MusicSource
{
    public LightAudioMusicSource()
    {
        SourceLink = "https://web.ligaudio.ru/mp3/";
        ParseTags = new()
        {
            {TrackSourceType.General, new TrackSourceParseRule("item", "div")}, // amplitude-song-container
            {TrackSourceType.Name, new TrackSourceParseRule("title", "span")},
            {TrackSourceType.Author, new TrackSourceParseRule("autor", "span")},
            {TrackSourceType.Duration, new TrackSourceParseRule("d", "span")},
            {TrackSourceType.Link, new TrackSourceParseRule("down", "a")}
        };
    } 

    public override string FormingLink(string userInput)
    {
        return $"{SourceLink}{userInput.Replace(" ", "%20")}";
    }
}