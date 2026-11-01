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


    public static async Task<Bitmap?> DownloadBitmapAsync(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;

        try
        {
            byte[] imageBytes = await _httpClient.GetByteArrayAsync(url);

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