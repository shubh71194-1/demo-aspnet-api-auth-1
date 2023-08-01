using JwtAuthManager.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthManager
{
    public class JwtTokenHandler
    {
        public const string JWT_SECURITY_KEY = "Authentication_jwttokenhandler_jwtsecuritykey";
        private const int JWT_TOKEN_VALIDITY_MINS = 60;
        private readonly List<User> _users;

        public JwtTokenHandler()
        {
            _users = new List<User>
            {
                new User{ UserName = "admin", Password = "admin@123", Role = "Admin" },
                new User{ UserName = "user01", Password = "user01", Role = "User" }
            };
        }

        public AuthenticationResponse? GenerateJwtToken(AuthenticationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
                return null;
            
            var user = 
                _users
                .Where(x => x.UserName == request.UserName &&
                        x.Password == request.Password).FirstOrDefault();

            if (user == null) return null;

            var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
            var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
            var clainIdentity = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Name, request.UserName),
                new Claim(ClaimTypes.Name, user.Role)
            });

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = clainIdentity,
                Expires = tokenExpiryTimeStamp,
                SigningCredentials = signingCredentials
            };

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
            var token = jwtSecurityTokenHandler.WriteToken(securityToken);

            return new AuthenticationResponse
            {
                UserName = user.UserName,
                ExpiresIn = (int) tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds,
                JwtToken = token,
                Role = user.Role
            };
        }
    }
}
