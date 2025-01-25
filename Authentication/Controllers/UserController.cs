using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Authentication.Models;
using Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private static List<User> _users = new List<User>
    {
        new User() { UserName = "Jack", Password = "Jack123", Role = "user" },
        new User() { UserName = "John", Password = "asdadas", Role = "admin" },
    };

    [HttpPost]
    [Route("login")]
    public IActionResult Login(User user)
    {
        User? fUser = _users.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);
        if (fUser is null)
            return Unauthorized();

        var claims = new List<Claim>()
            { new Claim(ClaimTypes.Name, fUser.UserName), new Claim(ClaimTypes.Role, fUser.Role) };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.AUDIENCE,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(3)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256)
        );
        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(jwt) , user = fUser.UserName });
}

    [HttpGet]
    [Route("check")]
    [Authorize(Roles = "admin")]
    public IActionResult Check()
    {
        var username = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
        string content = "Complete!";
        if (!username.IsNullOrEmpty())
            content += $"\n{username}";
        return Content(content, "text/plain");
    }
}