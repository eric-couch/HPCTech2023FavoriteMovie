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
    public IQueryable<MovieSearchResultItem>? movies { get; set; }

    public string searchTerm { get; set; }
    private int pageNum { get; set; } = 1;
    private int totalPages { get; set; }
    private int totalResults { get; set; } = 0;

    private async Task NextPage()
    {
        if (pageNum < totalPages) {
            pageNum++;
            await SearchOMDB();
        }
    }

    private async Task PreviousPage()
    {
        if (pageNum > 1)
        {
            pageNum--;
            await SearchOMDB();
        }
    }

    private async Task SearchOMDB()
    {
        movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}&page={pageNum}");
        if (movieResult is not null)
        {
            movies = movieResult.Search.AsQueryable();

            if (Double.TryParse(movieResult.totalResults, out double _total))
            {
                totalResults = (int)_total;
                totalPages = (int)Math.Ceiling(_total / 10);  // if 200 then 20, 202 you get 21
            } else
            {
                totalPages = 1;
            }
        }
        StateHasChanged();
    }
}
