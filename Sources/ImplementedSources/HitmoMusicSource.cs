namespace MusicService.Sources;

public class HitmoMusicSource: MusicSource
{
    public HitmoMusicSource()
    {
        UrlPrefix = "https://ru.hitmoz.org";
        ParseTags = new()
        {
            {TrackSourceType.General, new TrackSourceParseRule("tracks__item", "li")},
            {TrackSourceType.Name, new TrackSourceParseRule("track__title", "div")},
            {TrackSourceType.Author, new TrackSourceParseRule("track__desc", "div")},
            {TrackSourceType.Duration, new TrackSourceParseRule("track__fulltime", "div")},
            {TrackSourceType.Link, new TrackSourceParseRule("track__download-btn", "a")},
        };
    } 

    public override string FormingLink(string userInput)
    {
        return $"https://ru.hitmoz.org/search?q={userInput.Replace(" ", "%20")}";

        // if userInput for example:"limp bizkit", then => "limp%20bizkit"
        // and finally return => "https://ru.hitmoz.org/search?q=limp%20bizkit"
    }
}