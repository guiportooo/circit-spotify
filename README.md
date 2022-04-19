# circit-spotify

## The challenge

- Using C# (preferably .net core), demonstrate your knowledge of oAuth2 using the implicit flow to connect to the oAuth service of your preference (Facebook, Gitlab, Google, etc) without using that provider’s SDK.
- You can use whatever project type you prefer, for example maybe consider a console app to launch the browser and receive a callback!
- The application should allow a user to authenticate with the chosen provider.
- The application should, once the oAuth flow completes, use the user's authentication token to access the provider APIs to get some details of the user.
- Please make this app available on a public git repo such as GitHub.
- Any relevant information should be included in the README.md file and visible in GitHub.
- All files necessary to configure and/or execute the application must be in the GitHub repository.
- Bonus points for tests and production grade code!

## Decisions

I decided not to spend more thant 2 hours working on the challenge, as suggested, and for that reason I didn't have the time to try to develop a Console App interacting with the browser.

I chose to use a [.NET minimal API](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0) to interact with [Spotify's Web API](https://developer.spotify.com/documentation/web-api/reference/#/), using the [authorization code flow](https://developer.spotify.com/documentation/general/guides/authorization/code-flow/). This is a more secure option than the implicit grant flow.

## Possible improvements

- Validate the `state` information
- Refresh the `access_token`
- Write unit/integration tests

## Dependencies
- [.NET SDK 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Before you run the API

1. Log in to your account on [Spotify Dashboard](https://developer.spotify.com/dashboard/)
2. Create an App
3. Copy and paste your app's `Client ID` and `Client Secret` in the `appsettings.json` file
4. On the Dashboard, edit your app's settings to set the Redirect URIs as `https://localhost:7207/api/spotify/callback`

## Running locally

1. Go to the `/Circit.Spotify.Api` directory
2. Run the command `dotnet run`
3. The API will be available at _https://localhost:7207_

Alternatively, you can run the Circit.Spotify.Api project on your favorite IDE

## Interacting with the API

You can call the endpoints bellow to:

1. Log in to your Spotify account: `https://localhost:7207/api/login`
2. Retrieve your profile's information: `https://localhost:7207/api/me`
3. Retrieve your playlists: `https://localhost:7207/api/me/playlists`