using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var corsPolicy = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RPSDbContext>( options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) );
builder.Services.AddCors(options => {
  options.AddPolicy(name: corsPolicy,
    policyBuilder => {
      policyBuilder.WithOrigins("http://localhost:3000").AllowAnyHeader();
    });
});

var tokenValidationParameters = new TokenValidationParameters {
  ValidIssuer = builder.Configuration["Jwt:Issuer"],
  ValidAudience = builder.Configuration["Jwt:Audience"],
  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
};
builder.Services.AddAuthentication(options => {
  options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
  options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options => {
  options.TokenValidationParameters = tokenValidationParameters;
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();

var optionsBuilder = new DbContextOptionsBuilder<RPSDbContext>(); 
optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));

SecurityHandler securityHandler = new SecurityHandler(tokenValidationParameters);
RouteHandler routeHandler = new RouteHandler(securityHandler);

app.MapGet("/", () => { return "RPS Showdown API. Welcome!"; });

app.MapPost("/access", (UserRequest user, RPSDbContext db) => { return routeHandler.Login(user, db); });

app.MapGet("/validate", [Authorize] () => { return routeHandler.ValidUser(); });

app.MapPost("/stats", [Authorize] (StatsRequest user, RPSDbContext db) => { return routeHandler.Stats(user, db); });

app.MapPost("/create-session", [Authorize] (SessionRequest session, RPSDbContext db) => { return routeHandler.CreateSession(session, db); });

app.MapGet("/play-info", (RPSDbContext db) => { return routeHandler.GetPlayInfo(db); });

app.MapPost("/play", [Authorize] (PlayRequest play, RPSDbContext db) => { return routeHandler.Play(play, db); });

app.MapPost("/spectate", [Authorize] (SpectateRequest request, RPSDbContext db) => { return routeHandler.Spectate(request, db); });

app.Run();