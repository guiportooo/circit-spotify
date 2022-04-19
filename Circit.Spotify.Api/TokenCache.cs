namespace Circit.Spotify.Api;

public class TokenCache
{
    private const string CacheKey = "SpotifyToken";
    private readonly IMemoryCache _cache;

    public TokenCache(IMemoryCache cache) => _cache = cache;

    public void SetToken(TokenResponse tokenResponse) => _cache.Set(CacheKey, tokenResponse);

    public string GetAccessToken()
    {
        if (!_cache.TryGetValue<TokenResponse>(CacheKey, out var token))
        {
            throw new Exception("Failed to get access token");
        }
        
        return token.AccessToken;
    }
}