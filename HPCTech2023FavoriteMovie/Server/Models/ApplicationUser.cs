using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HPCTech2023FavoriteMovie.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Phone]
        public string? Phone { get; set; }

        public List<Movie> FavoriteMovies { get; set; } = new List<Movie>();

    }
}