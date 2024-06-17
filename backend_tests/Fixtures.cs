using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Moq;

public class RouteHandlerFixture {
    public RouteHandler RouteHandler { get; private set; }
    public RouteHandlerFixture() {
        var tokenParams = new TokenValidationParameters {
            ValidIssuer = "http://localhost:5000/",
            ValidAudience = "http://localhost:3000/",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("aksldfja;sdfjas;dfja;sdfiojasdfjasiodfjas;dfaklsdfj;asdfadfiojasd;fljkasd;lkfjasd;fiasodfjasdfiajsdfo;ajsdfasiojdfajsoidfjasodfaj;sdfasidfja;osdijfaiosdjfa;sjdfa;sndvilbcvlzxbvliuhiuarewhfiasdjcvaksdfnjvanflivaufidhhkaksdncxv,jhxcvbjlfuia"))
        };
        SecurityHandler securityHandler = new SecurityHandler(tokenParams);
        RouteHandler = new RouteHandler(securityHandler);
    }
}

public class DatabaseFixture {
    public Mock<RPSDbContext> MockContext { get; private set; }
    private DbSet<T> CreateMockSet<T>(List<T> data) where T : class {
        var queryableData = data.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();
        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator);
        mockSet.Setup(m => m.Add(It.IsAny<T>())).Callback<T>(data.Add);
        /* Will only need to remove for testing purposes; not sure if this is the right way to do it but 
        the code tested never uses remove so it will be fine in this case */
        mockSet.Setup(m => m.Remove(It.IsAny<T>())).Callback<T>((_) => data.RemoveAt(data.Count() - 1)); 

        return mockSet.Object;
    }
    public DatabaseFixture() {

        MockContext = new Mock<RPSDbContext>(new DbContextOptions<RPSDbContext>());

        var userData = new List<User>{
            new User { ID = 1, Username = "Anya", Password = "Password123"},
            new User { ID = 2, Username = "Milo", Password = "Password456" }
        };
        var mockUserSet = CreateMockSet(userData);

        MockContext.Setup(m => m.UserItems).Returns(mockUserSet);

        var levelData = new List<Level>{
            new Level { ID = -1, Name = "Player" },
            new Level { ID = 1, Name = "Beginner" },
            new Level { ID = 2, Name = "Intermediate" },
            new Level { ID = 3, Name = "Advanced" }
        };
        var mockLevelSet = CreateMockSet(levelData);

        MockContext.Setup(m => m.LevelItems).Returns(mockLevelSet);

        var sessionData = new List<Session>{
            new Session { ID = 1, UserID = 1, PlayerID = -1, LevelID = 1, StartedAt = DateTime.UtcNow },
            new Session { ID = 2, UserID = 1, PlayerID = -1, LevelID = 2, StartedAt = DateTime.UtcNow },
            new Session { ID = 3, UserID = 1, PlayerID = -1, LevelID = 3, StartedAt = DateTime.UtcNow },
            new Session { ID = 4, UserID = 1, PlayerID = 1, LevelID = 2, StartedAt = DateTime.UtcNow }
        };

        var mockSessionSet = CreateMockSet(sessionData);

        MockContext.Setup(m => m.SessionItems).Returns(mockSessionSet);

        var matchData = new List<Match>{
            new Match { ID = 1, UserID = 1, SessionID = 1, PlayerChoice = "rock", LevelChoice = "scissors", Result = "win" },
            new Match { ID = 2, UserID = 1, SessionID = 1, PlayerChoice = "rock", LevelChoice = "scissors", Result = "win" },
            new Match { ID = 3, UserID = 1, SessionID = 1, PlayerChoice = "rock", LevelChoice = "scissors", Result = "win" },
            new Match { ID = 4, UserID = 1, SessionID = 1, PlayerChoice = "scissors", LevelChoice = "rock", Result = "lose" },

            new Match { ID = 5, UserID = 1, SessionID = 2, PlayerChoice = "rock", LevelChoice = "scissors", Result = "win" },
            new Match { ID = 6, UserID = 1, SessionID = 2, PlayerChoice = "paper", LevelChoice = "paper", Result = "draw" },
            new Match { ID = 7, UserID = 1, SessionID = 2, PlayerChoice = "scissors", LevelChoice = "rock", Result = "lose" },

            new Match { ID = 8, UserID = 1, SessionID = 3, PlayerChoice = "rock", LevelChoice = "paper", Result = "lose" },
            new Match { ID = 9, UserID = 1, SessionID = 3, PlayerChoice = "scissors", LevelChoice = "paper", Result = "win" },
            new Match { ID = 10, UserID = 1, SessionID = 3, PlayerChoice = "rock", LevelChoice = "paper", Result = "lose" },
            new Match { ID = 11, UserID = 1, SessionID = 3, PlayerChoice = "scissors", LevelChoice = "paper", Result = "win" },

            new Match { ID = 12, UserID = 2, SessionID = 4, PlayerChoice = "rock", LevelChoice = "scissors", Result = "win" }
        };
        var mockMatchSet = CreateMockSet(matchData);

        MockContext.Setup(m => m.MatchItems).Returns(mockMatchSet);
    }
}