using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Services;

public class AuthOptions
{
    public const string ISSUER = "test";
    public const string AUDIENCE = "user";
    private const string KEY = "secretKey123qwe123qweasd123asd";
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}