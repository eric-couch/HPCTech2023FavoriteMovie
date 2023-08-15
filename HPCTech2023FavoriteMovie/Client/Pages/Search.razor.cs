using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
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
    private int page { get; set; } = 1;
    private int totalItems { get; set; } = 0;
    private List<MovieSearchResultItem> OMDBMovies { get; set; } = new List<MovieSearchResultItem>();
    private SfGrid<MovieSearchResultItem> movieGrid;
    private SfPager? Page;
    private List<MovieSearchResultItem> SelectedMovies { get; set; } = new();

    
    private async Task SearchOMDB()
    {
        movieResult = await Http.GetFromJsonAsync<MovieSearchResult>($"{OMDBAPIUrl}{OMDBAPIKey}&s={searchTerm}&page={page}");
        if (movieResult is not null)
        {
            OMDBMovies = movieResult.Search.ToList();
            totalItems = Int32.Parse(movieResult.totalResults);
        }
        
    }

    public async Task GetSelectedRows(RowSelectEventArgs<MovieSearchResultItem> args)
    {
        SelectedMovies = await movieGrid.GetSelectedRecordsAsync();
    }

    public async Task PageClick(PagerItemClickEventArgs args)
    {
        page = args.CurrentPage;
        await SearchOMDB();
    }

    public async Task ToolbarClickHandler(ClickEventArgs args)
    {
        if (args.Item.Id == "GridMovieAdd")
        {
            if (SelectedMovies is not null)
            {
                foreach (var movie in SelectedMovies)
                {
                    Movie newMovie = new Movie() { imdbId = movie.imdbID };
                    var res = await Http.PostAsJsonAsync("api/add-movie", newMovie);
                }
            }
        }
    }
}
