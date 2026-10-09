namespace MusicService.Sources;

public class HitmoMusicSource: MusicSource
{
    public HitmoMusicSource()
    {
        SourceLink = "https://ru.hitmoz.org/search?q=";
    } 

    public override string FormingLink(string userInput)
    {
        return $"{SourceLink}{userInput.Replace(" ", "%20")}";

        // if userInput for example:"limp bizkit", then => "limp%20bizkit"
        // and finally return => "https://ru.hitmoz.org/search?q=limp%20bizkit"
    }
}