namespace Circit.Spotify.Api;

public class SpotifyClient
{
    private readonly SpotifyConfig _config;
    private readonly HttpClient _httpClient;
    private readonly TokenCache _cache;

    public SpotifyClient(IOptions<SpotifyConfig> config, HttpClient httpClient, TokenCache cache)
    {
        _config = config.Value;
        _httpClient = httpClient;
        _cache = cache;
    }

    public string GetAuthorizeUrl()
    {
        var state = GenerateRandomString(16);
        return $"{_config.AuthorizeUrl}" +
               $"?client_id={_config.ClientId}" +
               "&response_type=code" +
               $"&redirect_uri={_config.RedirectUri}" +
               $"&scope={_config.Scope}" +
               $"&state={state}";
    }

    public async Task GetAccessToken(string code, string state)
    {
        var authorizationString = $"{_config.ClientId}:{_config.ClientSecret}";
        var basicToken = Convert.ToBase64String(Encoding.ASCII.GetBytes(authorizationString));

        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicToken);
        _httpClient.DefaultRequestHeaders.Add("Content_Type", "application/x-www-form-urlencoded");

        using var requestContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        {
            new("grant_type", "authorization_code"),
            new("code", code),
            new("redirect_uri", _config.RedirectUri)
        });

        var response = await _httpClient.PostAsync(new Uri(_config.TokenUrl), requestContent);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get access token");
        }

        var content = await response.Content.ReadAsStreamAsync();
        var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(content);

        if (tokenResponse is null)
        {
            throw new Exception("Failed to get access token");
        }

        _cache.SetToken(tokenResponse);
    }

    public async Task<dynamic?> GetCurrentUsersProfile()
    {
        SetAuthorizationHeader();

        var response = await _httpClient.GetAsync(new Uri($"{_config.BaseUrl}/me"));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get current user's profile");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<dynamic>(content);
    }

    public async Task<dynamic?> GetCurrentUsersPlaylists()
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync(new Uri($"{_config.BaseUrl}/me/playlists"));

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get current user's playlists");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<dynamic>(content);
    }

    private void SetAuthorizationHeader()
    {
        var accessToken = _cache.GetAccessToken();
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    private static string GenerateRandomString(int length)
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}