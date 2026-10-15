using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MusicService.Infrastructure;

internal sealed class HttpService
{
    public async Task<HtmlDocument> LoadHtml(string url)
    {
        try
        {
            var web = new HtmlWeb();
            var doc = await web.LoadFromWebAsync(url); 
            return doc;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}