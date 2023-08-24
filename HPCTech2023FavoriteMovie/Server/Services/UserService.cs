using HPCTech2023FavoriteMovie.Server.Controllers;
using HPCTech2023FavoriteMovie.Server.Data;
using HPCTech2023FavoriteMovie.Server.Models;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HPCTech2023FavoriteMovie.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserController> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
    }

    public async Task<UserDto> GetMovies(ClaimsPrincipal User)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        var movies = await _context.Users
            .Include(u => u.FavoriteMovies)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FavoriteMovies = u.FavoriteMovies
            }).FirstOrDefaultAsync(u => u.Id == user.Id);

        _logger.LogInformation("User {UserName} retreiving {Count} favorite movies.  Logged at {Placeholder:MMMM dd, yyyy}", user.UserName, movies.FavoriteMovies.Count, DateTimeOffset.UtcNow);

        return movies;
    }
}
