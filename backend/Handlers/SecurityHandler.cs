using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public interface ISecurityHandler {
  public bool UserExists(UserRequest user, RPSDbContext db);
  public bool AuthenticateUser(UserRequest user, RPSDbContext db);
}

public class SecurityHandler : ISecurityHandler {

  public TokenValidationParameters TokenParams { get; set; }

  public SecurityHandler(TokenValidationParameters tokenParams) {
    TokenParams = tokenParams;
  }

  public bool UserExists(UserRequest user, RPSDbContext db) {
    return db.UserItems.Any(entry => entry.Username == user.Username);
  }

  public bool AuthenticateUser(UserRequest user, RPSDbContext db) {
    return db.UserItems.Any(entry => entry.Username == user.Username && entry.Password == user.Password);
  }

  public string CreateToken(UserRequest user) {
    var tokenDescriptor = new SecurityTokenDescriptor{
      Subject = new ClaimsIdentity(new[]{
        new Claim("Id", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      }),
      Expires = DateTime.UtcNow.AddHours(1),
      Issuer = TokenParams.ValidIssuer,
      Audience = TokenParams.ValidAudience,
      SigningCredentials = new SigningCredentials(TokenParams.IssuerSigningKey, SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);
    return jwtToken;
  }
  // add unauthorized code path
}