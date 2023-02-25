using Enterprise.Solution.Email.Service;
using Enterprise.Solution.Service.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//TODO: This is not finished yet
namespace Enterprise.Solution.API.Controllers.Common
{
    /// <summary>
    /// Controller for authenticating users to access the API
    /// </summary>
    [Route("api/authentication")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        /// <summary>
        /// Method to attempt authentication for user
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync()
        {
            var user = ValidateUserCredentials();

            if (user == null)
            {
                return Unauthorized();
            }

            string issuer = Configuration.GetSection("Authentication")["Schemes:Swagger:ClaimsIssuer"]!;
            string audience = Configuration.GetSection("Authentication")["Schemes:Swagger:Audience"]!;
            string key = Configuration.GetSection("Authentication")["Schemes:Swagger:SecretForKey"]!;

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("user_name", user.UserName.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName.ToString()));
            claimsForToken.Add(new Claim("family_name", user.LastName.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                issuer,
                audience,
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                "authentication-controller@domain.local",
                "admin@domain.local",
                "Token created",
                $"Token created: {tokenToReturn}"
            ));

            return Ok(tokenToReturn);
        }

        //TODO: Create Service to login in via multiple ways
        private AuthorizedUser ValidateUserCredentials()
        {
            return new AuthorizedUser(
                1,
                "jennifer.allen",
                "Jennifer",
                "Allen");
        }
    }
}
