using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MusicService.Infrastructure;

internal sealed class HttpService
{
    public async Task<HtmlDocument> LoadHtml(string url)
    {
        var web = new HtmlWeb();
        return await web.LoadFromWebAsync(url);
    }
}