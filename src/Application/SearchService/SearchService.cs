using MusicService.Sources;
using System.Threading.Tasks;
using MusicService.Infrastructure;

namespace MusicService.Applic.SearchService;

// global class for search tracks on resources
public class SearchService
{
    private MusicSource _musicSource;
    private Parser _parser;
    private HttpService _httpService;

    public SearchService(MusicSource source)
    {
        _musicSource = source;
        _parser = new();
        _httpService = new();
    }

    public void ChangeSource(MusicSource souce)
    {
        _musicSource = souce;
    }

    public async Task<SearchResult> SearchTracks(string userInput)
    {
        var link = _musicSource.FormingLink(userInput);
        var doc = await _httpService.LoadHtml(link);

        return _parser.ParseSource(doc, _musicSource.ParseTags, _musicSource.UrlPrefix);
    }

}