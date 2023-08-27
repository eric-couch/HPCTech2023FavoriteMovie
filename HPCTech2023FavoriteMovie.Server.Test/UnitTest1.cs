using HPCTech2023FavoriteMovie.Server.Controllers;
using HPCTech2023FavoriteMovie.Server.Services;
using HPCTech2023FavoriteMovie.Shared;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace HPCTech2023FavoriteMovie.Server.Test;

public class Tests
{
    private readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetMovies_ShouldReturnUserDto_WhenUserExists()
    {
        // Arrange
        UserDto returUser = new UserDto
        {
            Id = "6f448473-acf0-442d-8d39-b7d844810fbe",
            UserName = "ericcouch@example.com",
            FirstName = "Eric",
            LastName = "Couch",
            FavoriteMovies = new List<Movie>() {
                    new Movie()
                    {
                        Id = 1018,
                        imdbId = "tt0372784"
                    },
                    new Movie()
                    {
                        Id = 1019,
                        imdbId = "tt0816692"
                    },
                    new Movie()
                    {
                        Id = 1020,
                        imdbId = "tt0482571"
                    }
                }
        };
        string? userName = "eric.couch@example.net";
        _userServiceMock.Setup(x => x.GetMovies(userName)).ReturnsAsync(returUser);

        UserController userController = new UserController(_userServiceMock.Object);

        /// Act
        var response = await userController.GetUserMovies(userName);
        var result = (OkObjectResult)response.Result;
        Assert.That(result.StatusCode, Is.EqualTo(200));
        var userDto = (UserDto)result.Value;

        // Assert
        Assert.That(userDto, Is.TypeOf<UserDto>());
        Assert.That(userDto.FirstName, Is.EqualTo("Eric"));
        Assert.That(userDto.LastName, Is.EqualTo("Couch"));
        Assert.That(userDto.FavoriteMovies[0].imdbId, Is.EqualTo("tt0372784"));
    }
}