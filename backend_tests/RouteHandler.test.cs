using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace backend_tests;
/*
These tests aren't comprehensive: they're just demonstrating my understanding of unit tests. 
*/
public class RouteHandlerTests : IClassFixture<DatabaseFixture>, IClassFixture<RouteHandlerFixture>
{
    DatabaseFixture DbFixture;
    RouteHandlerFixture RouteFixture;
    public RouteHandlerTests(DatabaseFixture dbFixture, RouteHandlerFixture routeFixture) {
        DbFixture = dbFixture;
        RouteFixture = routeFixture;
    }

    [Fact]
    public void Test_Valid_Login() {
        // Arrange
        UserDetails user = new UserDetails { Username = "Anya", Password = "Password123" };

        // Act
        var result = RouteFixture.RouteHandler.Login(user, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);

        // We don't really need to test that the token is what we expect; just that it's a string. If we were being more thorough we could generate the expected token but this is tricky because the function is not idempotent
        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public void Test_Valid_Register() {
        // Arrange
        UserDetails user = new UserDetails { Username = "Pedro", Password = "Password789" };
        int users = DbFixture.MockContext.Object.UserItems.Count();

        // Act
        var result = RouteFixture.RouteHandler.Login(user, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);

        // We don't really need to test that the token is what we expect; just that it's a string. If we were being more thorough we could generate the expected token but this is tricky because the function is not idempotent
        Assert.IsType<Ok<string>>(result);
        // We should also test the number of users has increased
        Assert.Equal(users+1, DbFixture.MockContext.Object.UserItems.Count());
    }

    [Fact]
    public void Test_Invalid_Login() {
        // Arrange
        UserDetails user = new UserDetails { Username = "Anya", Password = "Password456" };

        // Act
        var result = RouteFixture.RouteHandler.Login(user, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);

        // Likewise, just need to test that it is indeed of the specified type
        Assert.IsType<UnauthorizedHttpResult>(result);
    }

    [Fact]
    public void Test_Valid_Stats() {
        // Arrange
        StatsDetails user = new StatsDetails { Username = "Anya" };

        StatsInfo beginnerStats = new StatsInfo { WinRate = (float) 3/4, Ace = "rock", Nemesis = "rock", ChoiceDistribution = new ChoiceDistribution { Rock = (float) 3/4, Paper = 0.0f, Scissors = (float) 1/4}, LevelID = 1, LongestStreak = 3, Style = "none" };

        StatsInfo intermediateStats = new StatsInfo { WinRate = (float) 1/3, Ace = "rock", Nemesis = "rock", ChoiceDistribution = new ChoiceDistribution { Rock = (float) 1/3, Paper = (float) 1/3, Scissors = (float) 1/3 }, LevelID = 2, LongestStreak = 1, Style = "none" };

        StatsInfo advancedStats = new StatsInfo { WinRate = (float) 2/4, Ace = "scissors", Nemesis = "paper", ChoiceDistribution = new ChoiceDistribution { Rock = (float) 2/4, Paper = 0.0f, Scissors = (float) 2/4 }, LevelID = 3, LongestStreak = 1, Style = "none" };
        // Act
        var result = RouteFixture.RouteHandler.Stats(user, DbFixture.MockContext.Object);

        // Assert

        // Test that the result is of the correct type and the last has 3 items, one for each level
        Assert.NotNull(result);
        var okResult = result as Ok<List<StatsInfo>>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);
        List<StatsInfo> stats = okResult.Value;
        Assert.Equal(3, stats.Count());

        // Using FluentAssertions because we need to compare objects

        // Test Beginner stats are correct
        stats[0].Should().BeEquivalentTo(beginnerStats);

        // Test Intermediate stats are correct
        stats[1].Should().BeEquivalentTo(intermediateStats);

        // Test Advanced stats are correct
        stats[2].Should().BeEquivalentTo(advancedStats);
    }

    [Fact]
    public void Test_NonExistentUser_Cannot_Get_Stats() {
        // Arrange
        StatsDetails user = new StatsDetails { Username = "asfdasdf" };

        // Act
        var result = RouteFixture.RouteHandler.Stats(user, DbFixture.MockContext.Object);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public void Test_Valid_CreateSession() {
        // Arrange
        SessionDetails sessionDetails = new SessionDetails { LevelID = 1, PlayerID = -1, Username = "Anya" };
        int sessions = DbFixture.MockContext.Object.SessionItems.Count();

        // Act
        var result = RouteFixture.RouteHandler.CreateSession(sessionDetails, DbFixture.MockContext.Object);
        Console.WriteLine(result.GetType());
        // Assert
        Assert.NotNull(result);
        Assert.IsType<Ok<int>>(result);
        // Test the number of sessions has increased by 1, and thus a new session has been created
        Assert.Equal(sessions+1, DbFixture.MockContext.Object.SessionItems.Count());
    }

    [Fact]
    public void Test_NonExistentUser_Cannot_CreateSession() {
        // Arrange
        SessionDetails sessionDetails = new SessionDetails { LevelID = 1, PlayerID = -1, Username = "sfsdfsc" };
        int sessions = DbFixture.MockContext.Object.SessionItems.Count();

        // Act
        var result = RouteFixture.RouteHandler.CreateSession(sessionDetails, DbFixture.MockContext.Object);

        // Assert
        Assert.IsType<NotFound>(result);
    }

    [Fact]
    public void Test_Valid_Level1_Play() {
        // Arrange
        PlayDetails playDetails = new PlayDetails { PlayerChoice = "rock", SessionID = 1, Username = "Anya" };

        // Act
        var result = RouteFixture.RouteHandler.Play(playDetails, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);
        var okResult = result as Ok<PlayResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

        Assert.Equal("rock", okResult.Value.BotChoice);
        Assert.Equal("draw", okResult.Value.Result);

        // Clean up
        DbFixture.MockContext.Object.MatchItems.Remove(new Match { });
    }

    [Fact]
    public void Test_Invalid_Level1_Play() {
        // Arrange
        PlayDetails playDetails = new PlayDetails { PlayerChoice = "sdfsfd", SessionID = 1, Username = "Anya" };

        // Act
        var result = RouteFixture.RouteHandler.Play(playDetails, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequest<string>>(result);
    }

    [Fact]
    public void Test_Valid_Spectate() {
        // Arrange
        SpectateRequest spectateRequest = new SpectateRequest { Username = "Milo", SessionID = 4 };

        // Act
        var result = RouteFixture.RouteHandler.Spectate(spectateRequest, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);
        var okResult = result as Ok<SpectateResponse>;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);

        Assert.Equal("rock", okResult.Value.PlayerChoice);
        Assert.Equal("paper", okResult.Value.LevelChoice);
        Assert.Equal("lose", okResult.Value.Result);

        // Clean up
        DbFixture.MockContext.Object.MatchItems.Remove(new Match { });
    }

    [Fact]
    public void Test_InValid_Player_Spectate() {
         // Arrange
        SpectateRequest spectateRequest = new SpectateRequest { Username = "Milo", SessionID = 3 };

        // Act
        var result = RouteFixture.RouteHandler.Spectate(spectateRequest, DbFixture.MockContext.Object);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<BadRequest<string>>(result);
    }
}