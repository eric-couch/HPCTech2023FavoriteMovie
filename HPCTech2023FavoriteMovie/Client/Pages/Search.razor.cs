using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace HPCTech2023FavoriteMovie.Client.Pages;

public partial class Search
{
    [Inject]
    public HttpClient Http { get; set; } = new HttpClient();
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";
    public MovieSearchResult movieResult { get; set; }

    public string searchTerm { get; set; }


    private async Task SearchOMDB()
    {
        movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}");
        //if (movieResult is not null)
        //{

        //}
    }
}
