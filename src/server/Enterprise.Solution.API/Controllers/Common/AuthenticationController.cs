using Enterprise.Solution.Email.Service;
using Enterprise.Solution.Service.Models.Authorization;
using Enterprise.Solution.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//TODO: This is not finished yet
namespace Enterprise.Solution.API.Controllers.Common
{
    /// <summary>
    /// Controller for authenticating users to access the API
    /// </summary>
    [Route("authentication")]
    public class AuthenticationController : BaseController<AuthenticationController>
    {
        /// <summary>
        /// Constructor for the AuthenticationController
        /// </summary>
        /// <param name="solutionSettings"></param>
        /// <param name="mediator"></param>
        public AuthenticationController(IOptions<SolutionSettings> solutionSettings, IMediator mediator) : base(solutionSettings, mediator) { }

        /// <summary>
        /// DTO for authentication requests
        /// </summary>
        public class AuthenticationRequestBody
        {
            /// <summary>
            /// UserName
            /// </summary>
            public string? UserName { get; set; }
            /// <summary>
            /// Password
            /// </summary>
            public string? Password { get; set; }
        }

        /// <summary>
        /// Method to attempt authentication for user
        /// </summary>
        /// <returns></returns>
        [HttpPost("api/authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            var user = ValidateUserCredentials(
                authenticationRequestBody.UserName,
                authenticationRequestBody.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            string audience = base._solutionSettings.Authentication.Schemes.Keycloak.Audience;
            string authority = base._solutionSettings.Authentication.Schemes.Keycloak.Authority;
            string clientId = base._solutionSettings.Authentication.Schemes.Keycloak.ClientId;
            string clientSecret = base._solutionSettings.Authentication.Schemes.Keycloak.ClientSecret;

            var securityKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(clientSecret));
            var signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", user.UserId.ToString()));
            claimsForToken.Add(new Claim("user_name", user.UserName.ToString()));
            claimsForToken.Add(new Claim("given_name", user.FirstName.ToString()));
            claimsForToken.Add(new Claim("family_name", user.LastName.ToString()));
            claimsForToken.Add(new Claim("audience", audience));
            claimsForToken.Add(new Claim("authoriy", authority));

            var jwtSecurityToken = new JwtSecurityToken(
                clientId,
                audience,
                claimsForToken,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials
                );

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
        private AuthorizedUser ValidateUserCredentials(string? userName, string? password)
        {
            return new AuthorizedUser(
                1,
                "network-api-user@domain.local",
                "Network",
                "API User");
        }
    }
}
