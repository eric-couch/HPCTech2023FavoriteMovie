using HPCTech2023FavoriteMovie.Shared;
using HPCTech2023FavoriteMovie.Shared.Wrappers;

namespace HPCTech2023FavoriteMovie.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetMovies(string userName);
    Task<DataResponse<List<UserDto>>> GetUsers();
    Task<DataResponse<List<RoleDto>>> GetRoles(string id);
    Task<Response> AddRole(string role);
    //Task<Response> AddMovie(string imdbID);
    //Task<Response> RemoveMovie(string imdbID);
}
