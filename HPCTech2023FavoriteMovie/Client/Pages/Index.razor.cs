using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;

namespace HPCTech2023FavoriteMovie.Client.Pages;

public partial class Index
{
    [Inject]
    public HttpClient Http { get; set; } = new HttpClient();
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    public UserDto User = null;
    public List<OMDBMovie> userFavoriteMovies = new List<OMDBMovie>();
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public OMDBMovie? movieDetails { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            User = await Http.GetFromJsonAsync<UserDto>("api/User");
            // ?? is null coalescing operator
            if (User?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in User.FavoriteMovies)
                {
                    OMDBMovie omdbMovie = await Http.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    if (omdbMovie is not null)
                    {
                        userFavoriteMovies.Add(omdbMovie);
                    }
                }
            }
        }
        
    }
}
