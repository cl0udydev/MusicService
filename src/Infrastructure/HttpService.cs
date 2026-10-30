using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MusicService.Infrastructure;

internal sealed class HttpService
{
    public async Task<HtmlDocument?> LoadHtml(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
    {
        try
        {
            var web = new HtmlWeb();
            return await web.LoadFromWebAsync(url);
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, errorType);
            return null;
        }
    }
}