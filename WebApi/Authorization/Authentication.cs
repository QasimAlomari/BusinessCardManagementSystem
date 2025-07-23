using Domain.ViewModel;
using Domain.ViewModel.ApplicationUserViewModel;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApi.Authorization
{
    public class Authentication : IAuthentication<ApplicationUserModel>
    {

        private readonly IConfiguration _Configuration;

        public Authentication(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string GetJsonWebToken(ApplicationUserModel entity)
        {
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Configuration["Jwt:Key"]));
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,entity.ApplicationUserId),
                new Claim(JwtRegisteredClaimNames.GivenName,entity.ApplicationUserUsername),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

            var Token = new JwtSecurityToken
                (
                    issuer: _Configuration["Jwt:Issuer"],
                    audience: _Configuration["Jwt:Audience"],
                    claims: claims,
                    expires: Convert.ToDateTime(DateTime.Now.AddDays(1)),
                    signingCredentials: Credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
