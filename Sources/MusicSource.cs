namespace MusicService.Sources;

// music site/source class
public abstract class MusicSource
{
    public string SourceLink { get; set; }

    public MusicSource() // constructor-stub
    {
        SourceLink = "";
    }

    public abstract string FormingLink(string userInput);
}