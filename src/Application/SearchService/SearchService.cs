using MusicService.Sources;
using System.Threading.Tasks;
using MusicService.Infrastructure;

namespace MusicService.Applic.SearchService;

// global class for search tracks on resources
public class SearchService
{
    private IMusicSource _musicSource;
    private Parser _parser;
    private HttpService _httpService;

    public SearchService(IMusicSource source)
    {
        _musicSource = source;
        _parser = new();
        _httpService = new();
    }

    public void ChangeSource(IMusicSource souce)
    {
        _musicSource = souce;
    }

    public async Task<SearchResult> SearchTracks(string userInput)
    {
        var link = _musicSource.FormingLink(userInput);
        var doc = await _httpService.LoadHtml(link);
        
        SearchResult result;

        if (doc == null) result = new SearchResult(SearchResultType.Failed);
        else result = _parser.ParseSource(doc, _musicSource);

        return result;
    }

}