using Enterprise.Solution.Service.Models.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Enterprise.Solution.API.Controllers
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
        /// <param name="authenticationRequestBody"></param>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            string key = Configuration["Authentication:SecretForKey"] ?? string.Empty;
            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(key));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName.ToString()));
            claimsForToken.Add(new Claim("family_name", user.LastName.ToString()));

            var jwtSecurityToken = new JwtSecurityToken(
                Configuration["Authentication:Issuer"],
                Configuration["Authentication:Audience"],
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return Ok(tokenToReturn);
        }

        private AuthorizedUser ValidateUserCredentials(string? userName, string? password)
        {
            return new AuthorizedUser(
                1,
                userName ?? "",
                "Jennifer",
                "Allen");
        }
    }
}
