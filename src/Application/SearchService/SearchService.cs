using System.Threading.Tasks;
using MusicService.Infrastructure;
using MusicService.Sources;

namespace MusicService.Applic.SearchService;

public class SearchService
{
    private IMusicSource _musicSource;
    private readonly Parser _parser;
    private readonly HttpService _httpService;

    public SearchService(IMusicSource source)
    {
        _musicSource = source;
        _parser = new Parser();
        _httpService = new HttpService();
    }

    public void ChangeSource(IMusicSource source)
    {
        _musicSource = source;
    }

    public async Task<SearchResult> SearchTracks(string userInput)
    {
        var source = _musicSource;
        var link = source.FormingLink(userInput);
        var document = await _httpService.LoadHtmlAsync(link, NetworkErrorType.SearchError);

        if (document is null)
            return new SearchResult(SearchResultType.Failed, "track page not found");

        return _parser.ParseSource(document, source);
    }

    public async Task<SearchResult> SearchLyrics(string lyricsUrl)
    {
        var document = await _httpService.GetLyricsResponseAsync(lyricsUrl);

        if (document is null)
            return new SearchResult(SearchResultType.Failed, "lyrics page not found");

        return _parser.ParseLyrics(document);
    }


    public async Task<SearchResult> SearchCover(string coverUrl)
    {
        var coverBytes = await _httpService.DownloadImageBytesAsync(coverUrl);

        if (coverBytes is null)
            return new SearchResult(SearchResultType.Failed, "cover page not found");

        return new SearchResult(SearchResultType.Successfully, coverBytes, "successfully");
    }
}