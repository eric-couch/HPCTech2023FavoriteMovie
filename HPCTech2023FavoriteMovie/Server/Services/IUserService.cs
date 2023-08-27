using HPCTech2023FavoriteMovie.Shared;
using System.Security.Claims;

namespace HPCTech2023FavoriteMovie.Server.Services;

public interface IUserService
{
    Task<UserDto> GetMovies(String userName);
}
