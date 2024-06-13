using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public interface ISecurityHandler {
  public bool UserExists(UserDetails user, RPSDbContext db);
  public bool AuthenticateUser(UserDetails user, RPSDbContext db);
}

public class SecurityHandler : ISecurityHandler {

  public string Issuer { get; set; }
  public string Audience { get; set; }
  public byte[] Key { get; set; }

  public SecurityHandler(string issuer, string audience, byte[] key) {
    Issuer = issuer;
    Audience = audience;
    Key = key;
  }

  public bool UserExists(UserDetails user, RPSDbContext db) {
    return db.UserItems.Any(entry => entry.Username == user.Username);
  }

  public bool AuthenticateUser(UserDetails user, RPSDbContext db) {
    return db.UserItems.Any(entry => entry.Username == user.Username && entry.Password == user.Password);
  }

  public string CreateToken(UserDetails user) {
    var tokenDescriptor = new SecurityTokenDescriptor{
      Subject = new ClaimsIdentity(new[]{
        new Claim("Id", Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      }),
      Expires = DateTime.UtcNow.AddMinutes(5),
      Issuer = Issuer,
      Audience = Audience,
      SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Key), SecurityAlgorithms.HmacSha512Signature)
    };
    var tokenHandler = new JwtSecurityTokenHandler();
    var token = tokenHandler.CreateToken(tokenDescriptor);
    var jwtToken = tokenHandler.WriteToken(token);
    return jwtToken;
  }
  // add unauthorized code path
}