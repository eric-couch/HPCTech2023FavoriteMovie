using Duende.IdentityServer.EntityFramework.Options;
using HPCTech2023FavoriteMovie.Server.Models;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HPCTech2023FavoriteMovie.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<Movie> Movies => Set<Movie>();
    }
}