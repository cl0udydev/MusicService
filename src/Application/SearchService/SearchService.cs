using MusicService.Sources;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace MusicService.Applic.SearchService;

// global class for search tracks on resources
public class SearchService
{
    private MusicSource _musicSource;
    private Parser _parser;

    public SearchService(MusicSource source)
    {
        _musicSource = source;
        _parser = new();
    }

    public void ChangeSource(MusicSource souce)
    {
        _musicSource = souce;
    }

    public async Task<SearchResult> SearchTracks(string userInput)
    {
        var web = new HtmlWeb();
        var link = _musicSource.FormingLink(userInput);
        var doc = await web.LoadFromWebAsync(link);

        return _parser.ParseSource(doc, _musicSource.ParseTags, _musicSource.UrlPrefix);
    }

}