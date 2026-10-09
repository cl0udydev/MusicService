using MusicService.Sources;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace MusicService.App.SearchService;

// global class for search tracks on resources
public class SearchService
{
    private MusicSource _musicSource;

    public SearchService(MusicSource source)
    {
        _musicSource = source;
    }

    public SearchResult SearchTracks(string userInput) // function-stub
    {
        return new SearchResult(SearchResultType.Successfully); 
    }
    
    private async Task<HtmlDocument> TakeRequest(string userInput)
    {
        var web = new HtmlWeb();
        var link = _musicSource.FormingLink(userInput);
        var doc = await web.LoadFromWebAsync(link);
        return doc;
    }
}