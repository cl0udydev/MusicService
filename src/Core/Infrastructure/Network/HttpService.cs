using System;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace MusicService.Core.Infrastructure.Network;

internal sealed class HttpService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    
    static HttpService()
    {
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "cl0udydevMusicService");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
        );
    }


    public async Task<HtmlDocument?> LoadHtmlAsync(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
    {
        try
        {
            string htmlContent = await _httpClient.GetStringAsync(url);

            var document = new HtmlDocument();
            document.LoadHtml(htmlContent);
            
            return document;
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, errorType);
            return null; 
        }
    }


    public async Task<JsonDocument?> GetJsonResponseAsync(
        string url, IReadOnlyDictionary<string, string>? headers, NetworkErrorType errorType = NetworkErrorType.SearchError
    )
    {
        try
        {   
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    request.Headers.TryAddWithoutValidation(
                        header.Key,
                        header.Value
                    );
                }
            }

            using var response = await _httpClient.SendAsync(request);
            

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound) 
                return null;
                    
            if (!response.IsSuccessStatusCode)
                return null;


            string jsonResponse = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(jsonResponse);

            return doc;
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, errorType);
            return null;
        }
    }


    public async Task<byte[]?> DownloadImageBytesAsync(string url, NetworkErrorType errorType = NetworkErrorType.SearchError)
    {
        try
        {
            var imageBytes = await _httpClient.GetByteArrayAsync(url);

            return imageBytes;
        }
        catch (Exception ex)
        {
            NetworkErrorsSystem.EmitNetworkError(ex, errorType);
            return null;
        }
    }
}