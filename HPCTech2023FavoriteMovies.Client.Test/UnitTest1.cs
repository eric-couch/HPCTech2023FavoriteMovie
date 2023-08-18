using RichardSzalay.MockHttp;
using HPCTech2023FavoriteMovie.Client.HttpRepository;

namespace HPCTech2023FavoriteMovies.Client.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test_GetMOvies_ReturnUserAndFavoriteMovies()
    {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        // Mock api/User end point which returns UserDto
        // also mock the OMDB api returns for individual movies
        string testUserResponse = """
            {"id": "82740fca-8491-47e1-ba8e-00cce84fba19","userName": "eric.couch@example.com","firstName": "Eric","lastName": "Couch","favoriteMovies": [{"id": 1,"imdbId": "tt9603212"},{"id": 3,"imdbId": "tt7984734"}]}
            """;
        string testDeadReckoningResponse = """
            {"Title": "Mission: Impossible - Dead Reckoning Part One","Year": "2023","Rated": "PG-13","Released": "11 Jul 2023","Runtime": "163 min","Genre": "Action, Adventure, Thriller","Director": "Christopher McQuarrie","Writer": "Bruce Geller, Erik Jendresen, Christopher McQuarrie","Actors": "Tom Cruise, Hayley Atwell, Ving Rhames","Plot": "Ethan Hunt and his IMF team must track down a dangerous weapon before it falls into the wrong hands.","Language": "English, French, Italian, Russian","Country": "United States","Awards": "1 nomination","Poster": "https://m.media-amazon.com/images/M/MV5BYzFiZjc1YzctMDY3Zi00NGE5LTlmNWEtN2Q3OWFjYjY1NGM2XkEyXkFqcGdeQXVyMTUyMTUzNjQ0._V1_SX300.jpg","Ratings": [{"Source": "Internet Movie Database","Value": "8.1/10"},{"Source": "Rotten Tomatoes","Value": "96%"},{"Source": "Metacritic","Value": "81/100"}],"Metascore": "81","imdbRating": "8.1","imdbVotes": "63,077","imdbID": "tt9603212","Type": "movie","DVD": "N/A","BoxOffice": "$83,795,991","Production": "N/A","Website": "N/A","Response": "True"}
            """;
        string testTheLighthouseResponse = """
            {"Title": "The Lighthouse","Year": "2019","Rated": "R","Released": "01 Nov 2019","Runtime": "109 min","Genre": "Drama, Fantasy, Horror","Director": "Robert Eggers","Writer": "Robert Eggers, Max Eggers","Actors": "Robert Pattinson, Willem Dafoe, Valeriia Karaman","Plot": "Two lighthouse keepers try to maintain their sanity while living on a remote and mysterious New England island in the 1890s.","Language": "English","Country": "United States, Canada, Brazil","Awards": "Nominated for 1 Oscar. 33 wins & 134 nominations total","Poster": "https://m.media-amazon.com/images/M/MV5BZmE0MGJhNmYtOWNjYi00Njc5LWE2YjEtMWMxZTVmODUwMmMxXkEyXkFqcGdeQXVyMTkxNjUyNQ@@._V1_SX300.jpg","Ratings": [{"Source": "Internet Movie Database","Value": "7.4/10"},{"Source": "Rotten Tomatoes","Value": "90%"},{"Source": "Metacritic","Value": "83/100"}],"Metascore": "83","imdbRating": "7.4","imdbVotes": "235,111","imdbID": "tt7984734","Type": "movie","DVD": "18 Oct 2019","BoxOffice": "$10,867,104","Production": "N/A","Website": "N/A","Response": "True"}
            """;

        mockHttp.When("https://localhost:7026/api/User")
            .Respond("application/json", testUserResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt9603212")
            .Respond("application/json", testDeadReckoningResponse);
        mockHttp.When("https://www.omdbapi.com/?apikey=86c39163&i=tt7984734")
            .Respond("application/json", testTheLighthouseResponse);

        var client = mockHttp.ToHttpClient();
        client.BaseAddress = new Uri("https://localhost:7026/");
        var userMoviesHttpRespository = new UserMoviesHttpRespository(client);

        // Act
        var response = await userMoviesHttpRespository.GetMovies();
        var movies = response.Data;


        // Assert 
        Assert.That(movies.Count(), Is.EqualTo(2));
        Assert.That(movies[0].Title, Is.EqualTo("Mission: Impossible - Dead Reckoning Part One"));
        Assert.That(movies[0].Year, Is.EqualTo("2023"));
        Assert.That(movies[1].Title, Is.EqualTo("The Lighthouse"));
        Assert.That(movies[1].Year, Is.EqualTo("2019"));
    }
}