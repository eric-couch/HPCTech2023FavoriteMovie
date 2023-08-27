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

    public async Task<DataResponse<List<OMDBMovie>>> GetMovies(string userName)
    {
        try
        {
            var MovieDetails = new List<OMDBMovie>();
            UserDto User = await _httpClient.GetFromJsonAsync<UserDto>($"api/User?userName={userName}");
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

    public async Task<DataResponse<List<UserDto>>> GetUsers()
    {
        try
        {
            var users = await _httpClient.GetFromJsonAsync<List<UserDto>>("api/get-users");
            // package in dataresponse
            return new DataResponse<List<UserDto>>() {
                Data = users,
                Message = "Success",
                Succeeded = true
            };

        } catch (Exception ex)
        {
            return new DataResponse<List<UserDto>>()
            {
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } },
                Data = new List<UserDto>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
    }

    public async Task<DataResponse<List<RoleDto>>> GetRoles(string userId)
    {
        try
        {
            var roles = await _httpClient.GetFromJsonAsync<List<RoleDto>>($"api/get-roles/{userId}");
            // package in dataresponse
            return new DataResponse<List<RoleDto>>()
            {
                Data = roles,
                Message = "Success",
                Succeeded = true
            };

        }
        catch (Exception ex)
        {
            return new DataResponse<List<RoleDto>>()
            {
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } },
                Data = new List<RoleDto>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
    }

    public async Task<Response> AddRole(string role)
    {
        try
        {
            RoleDto newRole = new RoleDto()
            {
                Name = role
            };
            var res = await _httpClient.PostAsJsonAsync<RoleDto>("api/add-role", newRole);
            if (res.IsSuccessStatusCode)
            {
                return new Response()
                {
                    Message = "Success",
                    Succeeded = true
                };
            }
            else
            {
                return new Response()
                {
                    Message = "Failed",
                    Succeeded = false,
                    Errors = new Dictionary<string, string[]> { { "Failed to add Role", new string[] { "Failed to add Role" } } }
                };
            }

        }
        catch (Exception ex)
        {
            return new DataResponse<List<UserDto>>()
            {
                Errors = new Dictionary<string, string[]> { { ex.Message, new string[] { ex.Message } } },
                Data = new List<UserDto>(),
                Message = ex.Message,
                Succeeded = false
            };
        }
    }
}
