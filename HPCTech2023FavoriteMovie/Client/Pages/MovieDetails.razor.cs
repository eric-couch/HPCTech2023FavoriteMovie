using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;

namespace HPCTech2023FavoriteMovie.Client.Pages;

public partial class MovieDetails
{
    [Parameter]
    public OMDBMovie? Movie { get; set; }
    [Parameter]
    public EventCallback<OMDBMovie> OnRemoveFavoriteMovie { get; set; }

    private async Task RemoveFavoriteMovie(OMDBMovie movie)
    {
        await OnRemoveFavoriteMovie.InvokeAsync(movie);
    }
}
