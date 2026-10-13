namespace MusicService.Sources;

public class LightAudioMusicSource: MusicSource
{
    public LightAudioMusicSource()
    {
        UrlPrefix = "";
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
        return $"https://web.ligaudio.ru/mp3/{userInput.Replace(" ", "%20")}";
    }
}