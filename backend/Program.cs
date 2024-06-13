using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var corsPolicy = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RPSDbContext>( options => options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")) );
builder.Services.AddCors(options => {
  options.AddPolicy(name: corsPolicy,
    policy => {
      policy.WithOrigins("http://localhost:3000")
      .AllowAnyHeader();
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

app.MapPost("/access", (UserDetails user, RPSDbContext db) => { return routeHandler.Login(user, db); });

app.Run();