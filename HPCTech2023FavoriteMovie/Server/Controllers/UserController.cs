using HPCTech2023FavoriteMovie.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.EntityFrameworkCore;
using HPCTech2023FavoriteMovie.Server.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HPCTech2023FavoriteMovie.Server.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<ActionResult<UserDto>> GetUserMovies()
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

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    [HttpGet]
    [Authorize(Roles="admin")]
    [Route("api/get-users")]
    public async Task<List<UserDto>> GetUsers()
    {

        var users = await (from u in _context.Users
                           select new UserDto
                           {
                               Id = u.Id,
                               UserName = u.UserName,
                               FirstName =u.FirstName,
                               LastName = u.LastName
                           }).ToListAsync();

        if (users is not null )
        {
            return users;
        } else
        {
            return new List<UserDto>();
        }
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [Route("api/get-roles/{id}")]
    public async Task<List<string>> GetRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is not null)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        } else { return new List<string>(); }

    }

    [HttpPost]
    [Route("api/add-movie")]
    public async Task<ActionResult> AddMovie([FromBody] Movie movie)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (user is null) {
            return NotFound();
        }
        user.FavoriteMovies.Add(movie);
        _context.SaveChanges();
        return Ok();
    }

    [HttpPost]
    [Route("api/remove-movie")]
    public async Task<ActionResult> RemoveMovie([FromBody] Movie movie)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (user is null)
        {
            return NotFound();
        }
        var movieToRemove = _context.Users.Include(u => u.FavoriteMovies)
            .FirstOrDefault(u => u.Id == user.Id)
            .FavoriteMovies.FirstOrDefault(m => m.imdbId == movie.imdbId);
        _context.Movies.Remove(movieToRemove);
        _context.SaveChanges();
        return Ok();
    }

}
