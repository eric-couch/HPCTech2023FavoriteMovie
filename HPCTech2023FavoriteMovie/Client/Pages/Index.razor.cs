using HPCTech2023FavoriteMovie.Client.HttpRepository;
using HPCTech2023FavoriteMovie.Shared;
using HPCTech2023FavoriteMovie.Shared.Wrappers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.PivotView;
using System.Net.Http.Json;

namespace HPCTech2023FavoriteMovie.Client.Pages;

public partial class Index
{
    [Inject]
    public HttpClient Http { get; set; } = new HttpClient();
    [Inject]
    public AuthenticationStateProvider AuthenticationStateProvider { get; set; }
    [Inject]
    public IUserMoviesHttpRepository UserMovieHttpRepository { get; set; }
    //public UserDto User = null;
    public List<OMDBMovie> userFavoriteMovies = new List<OMDBMovie>();
    public OMDBMovie? movieDetails { get; set; }
    private SfToast? ToastObj;
    private string? toastContent = string.Empty;
    private string? toastSuccess = "e-toast-success";
    public int numCols = 6;
    protected override async Task OnInitializedAsync()
    {
        var UserAuth = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Identity;
        if (UserAuth is not null && UserAuth.IsAuthenticated)
        {
            DataResponse<List<OMDBMovie>> dataResponse = await UserMovieHttpRepository.GetMovies();
            if (dataResponse.Succeeded)
            {
                userFavoriteMovies = dataResponse.Data;
                StateHasChanged();
            }else
            {
                toastContent = $"Error: attempt to get user and favorite movies failed.";
                toastSuccess = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100);
                await ToastObj.ShowAsync();
            }
        }
        
    }

    private async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        try
        {
            Movie newMovie = new Movie { imdbId = movie.imdbID };
            var res = await Http.PostAsJsonAsync("api/remove-movie", newMovie);
            userFavoriteMovies.Remove(movie);
            StateHasChanged();
            if (!res.IsSuccessStatusCode)
            {
                toastContent = $"Failed to remove movie {movie.Title}";
                toastSuccess = "e-toast-warning";
                StateHasChanged();
                await Task.Delay(100);
                await ToastObj.ShowAsync();
            }
        } catch (Exception ex)
        {
            toastContent = $"Failed to remove movie {movie.Title}";
            toastSuccess = "e-toast-warning";
            StateHasChanged();
            await Task.Delay(100);
            await ToastObj.ShowAsync();
        }
        
    }
}
