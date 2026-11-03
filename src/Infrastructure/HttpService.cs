using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.IO;
using System.Net.Http;
using Avalonia.Media.Imaging;

namespace MusicService.Infrastructure;

internal sealed class HttpService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    
    static HttpService()
    {
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
        );
    }


    public async Task<HtmlDocument?> LoadHtmlAsync(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
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


    public async Task<Bitmap?> DownloadBitmapAsync(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
    {
        try
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(url);

            using var memoryStream = new MemoryStream(imageBytes);

            return new Bitmap(memoryStream);
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, errorType);
            return null;
        }
    }
}