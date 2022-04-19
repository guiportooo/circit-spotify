namespace Circit.Spotify.Api;

public class SpotifyConfig
{
    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
    public string RedirectUri { get; set; } = null!;
    public string Scope { get; set; } = null!;
    public string AuthorizeUrl { get; set; } = null!;
    public string TokenUrl { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
}