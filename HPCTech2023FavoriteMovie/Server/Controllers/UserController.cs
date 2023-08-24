using HPCTech2023FavoriteMovie.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.EntityFrameworkCore;
using HPCTech2023FavoriteMovie.Server.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using HPCTech2023FavoriteMovie.Server.Services;

namespace HPCTech2023FavoriteMovie.Server.Controllers;

public class UserController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(  ApplicationDbContext context, 
                            UserManager<ApplicationUser> userManager, 
                            RoleManager<IdentityRole> roleManager, 
                            ILogger<UserController> logger, 
                            IUserService userService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    [Route("api/User")]
    public async Task<ActionResult<UserDto>> GetUserMovies()
    {
        var user = await _userService.GetMovies(User);
        if (user is null)
        {
            _logger.LogWarning("User object not found for {UserName}", User?.Identity?.Name ?? "Not Found");
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
    public async Task<List<RoleDto>> GetRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user is not null)
        {
            IList<string> roles = await _userManager.GetRolesAsync(user);
            var retRoles =  (from r in roles
                            select new RoleDto
                            {
                                Name = r
                            }).ToList();
            return retRoles;
        } else { return new List<RoleDto>(); }

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

    [HttpPost]
    [Route("api/add-role")]
    public async Task<ActionResult> AddRole([FromBody] RoleDto role)
    {
        IdentityRole identityRole = new IdentityRole()
        {
            Name = role.Name
        };
        await _roleManager.CreateAsync(identityRole);
        return Ok();
    }

}
