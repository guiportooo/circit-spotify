var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Configure<SpotifyConfig>(builder.Configuration.GetSection("Spotify"))
    .AddMemoryCache()
    .AddScoped<TokenCache>()
    .AddHttpClient<SpotifyClient>();

var app = builder.Build();

app.MapGet("/api/login", ([FromServices] SpotifyClient spotifyClient) =>
{
    var url = spotifyClient.GetAuthorizeUrl();
    return Results.Redirect(url);
});

app.MapGet("/api/spotify/callback", async (string code,
    string state,
    [FromServices] SpotifyClient spotifyClient) =>
{
    try
    {
        await spotifyClient.GetAccessToken(code, state);
        return Results.Ok();
    }
    catch
    {
        return Results.Unauthorized();
    }
});

app.MapGet("/api/me", async ([FromServices] SpotifyClient spotifyClient) =>
{
    try
    {
        var me = await spotifyClient.GetCurrentUsersProfile();
        return me is null ? Results.Problem("Failed to get current user's profile") : Results.Json(me);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.MapGet("/api/me/playlists", async ([FromServices] SpotifyClient spotifyClient) =>
{
    try
    {
        var playlists = await spotifyClient.GetCurrentUsersPlaylists();
        return playlists is null ? Results.Problem("Failed to get current user's playlists") : Results.Json(playlists);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
});

app.Run();