using HPCTech2023FavoriteMovie.Shared;
using HPCTech2023FavoriteMovie.Shared.Wrappers;

namespace HPCTech2023FavoriteMovie.Client.HttpRepository;

public interface IUserMoviesHttpRepository
{
    Task<DataResponse<List<OMDBMovie>>> GetMovies();
    //Task<Response> AddMovie(string imdbID);
    //Task<Response> RemoveMovie(string imdbID);
}
