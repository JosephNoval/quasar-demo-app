using DEMO_SERVER.Core.DAL.Entities.Master;
using DEMO_SERVER.Core.Factories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace DEMO.API.Models
{
    public class SignInModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Msg { get; private set; }
        public User User { get; private set; }
        public string Token { get; private set; }

        public bool Login()
        {
            var repo = new RepositoryFactory().CreateUserRepository();
            User = repo.GetUser(Username, Password);
            if (User == null)
            {
                Msg = "Invalid Credentials";
                return false;
            }

            Token = CreateToken();
            Msg = "Success";
            return true;
        }

        public string CreateToken()
        {

            DateTime issuedAt = DateTime.UtcNow;
            DateTime expiry = DateTime.UtcNow.AddDays(30);

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var tokenHandler = new JwtSecurityTokenHandler();

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, User.Roles),
                new Claim(ClaimTypes.Name, User.DisplayName)
            });

            var secret = ConfigurationManager.AppSettings["jwtSecret"];
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secret));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        issuer: "http://localhost:50385",
                        audience: "http://localhost:50385",
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expiry,
                        signingCredentials: signingCredentials
                        );

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}