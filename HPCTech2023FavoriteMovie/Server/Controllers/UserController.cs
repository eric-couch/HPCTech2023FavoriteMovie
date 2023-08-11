using HPCTech2023FavoriteMovie.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.EntityFrameworkCore;
using HPCTech2023FavoriteMovie.Server.Data;
using System.Security.Claims;

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
        var user = await _context.Users
            .Include(u => u.FavoriteMovies)
            .Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FirstName = u.FirstName,
                LastName = u.LastName,
                FavoriteMovies = u.FavoriteMovies
            }).FirstOrDefaultAsync(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier));

        if (user is null)
        {
            return NotFound();
        }
        return Ok(user);
    }

}
