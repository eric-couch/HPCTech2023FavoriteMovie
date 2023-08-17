using HPCTech2023FavoriteMovie.Client.Pages;
using HPCTech2023FavoriteMovie.Shared;
using HPCTech2023FavoriteMovie.Shared.Wrappers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace HPCTech2023FavoriteMovie.Client.HttpRepository;

public class UserMoviesHttpRespository : IUserMoviesHttpRepository
{
    public readonly HttpClient _httpClient;
    private readonly string OMDBAPIUrl = "https://www.omdbapi.com/?apikey=";
    private readonly string OMDBAPIKey = "86c39163";

    public UserMoviesHttpRespository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<DataResponse<List<OMDBMovie>>> GetMovies()
    {
        try
        {
            var MovieDetails = new List<OMDBMovie>();
            UserDto User = await _httpClient.GetFromJsonAsync<UserDto>("api/User");
            if (User?.FavoriteMovies?.Any() ?? false)
            {
                foreach (var movie in User.FavoriteMovies)
                {
                    OMDBMovie omdbMovie = await _httpClient.GetFromJsonAsync<OMDBMovie>($"{OMDBAPIUrl}{OMDBAPIKey}&i={movie.imdbId}");
                    MovieDetails.Add(omdbMovie);
                }
                return new DataResponse<List<OMDBMovie>>()
                {
                    Data = MovieDetails,
                    Message = "Success",
                    Succeeded = true
                };
            }
            return new DataResponse<List<OMDBMovie>>()
            {
                Data = MovieDetails,
                Message = "Success",
                Succeeded = true
            };
        } 
        //catch (HttpRequestException ex)
        //{
        //    if (ex.Message.Contains(HttpStatusCode.Unauthorized.ToString())) {
        //        return new DataResponse<List<OMDBMovie>>()
        //        {
        //            Errors = new Dictionary<string, string[]> { { "Not Authorize", new string[] { "User does not have uathorized" } } },
        //            Data = new List<OMDBMovie>(),
        //            Message = "Not Authorized",
        //            Succeeded = false
        //        };
        //    }
        //}
        catch (NotSupportedException)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { "Not Supported", new string[] { "The content type is not supported" } } },
                Data = new List<OMDBMovie>(),
                Message = "Not Supported",
                Succeeded = false
            };
        }
        //catch (JsonException)
        //{

        //}
        catch (Exception ex)
        {
            return new DataResponse<List<OMDBMovie>>()
            {
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } },
                Data = new List<OMDBMovie>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
        

    }
}
