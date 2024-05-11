using EducationPortalApp.Dtos.AppUserDtos;
using EducationPortalApp.Dtos.TokenDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EducationPortalApp.Shared.Utilities.Security
{
    public class JwtTokenGenerator
    {
        public static TokenResponseDto GenerateToken(AppUserDto appUserDto, IList<string> roles)
        {
            //Security Key'in simetriği
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var expireDate = DateTime.UtcNow.AddMinutes(5555555);
            //Şifrelenmiş kimlik
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            List<Claim> myClaims = new List<Claim>();
            if (roles.Count > 0)
            {
                foreach (var claim in roles)
                {
                    myClaims.Add(new Claim(ClaimTypes.Role, claim));
                }
            }
            if (appUserDto.Id != null)
            {
                myClaims.Add(new Claim(ClaimTypes.NameIdentifier, appUserDto.Id.ToString()));
            }
            if (!string.IsNullOrEmpty(appUserDto.Username))
            {
                myClaims.Add(new Claim(ClaimTypes.Name, appUserDto.Username));
            }
            if (!string.IsNullOrEmpty(appUserDto.Email))
            {
                myClaims.Add(new Claim("Email", appUserDto.Email));
            }
            JwtSecurityToken token = new JwtSecurityToken(issuer: JwtTokenDefaults.ValidIssuer, audience: JwtTokenDefaults.ValidAudience, claims: myClaims, notBefore: DateTime.UtcNow, expires: expireDate, signingCredentials: credentials);

            //Token oluşturucu sınıf
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return new TokenResponseDto(handler.WriteToken(token), expireDate);
        }
    }
}
