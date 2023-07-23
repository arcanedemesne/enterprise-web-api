using Enterprise.Solution.API.Models;
using Enterprise.Solution.Email.Service;
using Enterprise.Solution.Service.Models.Authorization;
using Enterprise.Solution.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    [Route("api/authentication")]
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
            public string? Username { get; set; }
            /// <summary>
            /// Password
            /// </summary>
            public string? Password { get; set; }
        }

        /// <summary>
        /// Keycloak Response
        /// </summary>
        public class KeycloakResponse
        {
            /// <summary>
            /// access_token
            /// </summary>
            public string access_token { get; set; } = null!;
            /// <summary>
            /// expires_in
            /// </summary>
            public string expires_in { get; set; } = null!;
            /// <summary>
            /// refresh_expires_in
            /// </summary>
            public string refresh_expires_in { get; set; } = null!;
            /// <summary>
            /// refresh_token
            /// </summary>
            public string refresh_token { get; set; } = null!;
            /// <summary>
            /// token_type
            /// </summary>
            public string token_type { get; set; } = null!;
            /// <summary>
            /// id_token
            /// </summary>
            public string id_token { get; set; } = null!;
            /// <summary>
            /// session_state
            /// </summary>
            public string session_state { get; set; } = null!;
            /// <summary>
            /// scope
            /// </summary>
            public string scope { get; set; } = null!;
        }

        /// <summary>
        /// Method to attempt authentication for user
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            var clientId = base._solutionSettings.Authentication.Schemes.Keycloak.ClientId;
            var clientSecret = base._solutionSettings.Authentication.Schemes.Keycloak.ClientSecret;
            var audience = base._solutionSettings.Authentication.Schemes.Keycloak.Audience;

            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.TokenExchange;
            var client = new HttpClient();

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "openid"),
                new KeyValuePair<string, string>("username", authenticationRequestBody.Username!),
                new KeyValuePair<string, string>("password", authenticationRequestBody.Password!),
            };
            var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<KeycloakResponse>(response.Content.ReadAsStringAsync().Result);
                var id_token = content?.id_token;
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(id_token);

                var token_username = GetClaimFromToken(jwtSecurityToken, "preferred_username")?.Value;
                var token_full_name = GetClaimFromToken(jwtSecurityToken, "name")?.Value;
                var token_given_name = GetClaimFromToken(jwtSecurityToken, "given_name")?.Value;
                var token_family_name = GetClaimFromToken(jwtSecurityToken, "family_name")?.Value;
                var token_email_address = GetClaimFromToken(jwtSecurityToken, "email")?.Value;
                var token_expiry = GetClaimFromToken(jwtSecurityToken, "exp")?.Value;
                var email_verified = GetClaimFromToken(jwtSecurityToken, "email_verified")?.Value;

                var claimsForToken = new List<Claim>() {
                    new Claim("username", token_username!),
                    new Claim("full_name", token_full_name!),
                    new Claim("given_name", token_given_name!),
                    new Claim("family_name", token_family_name!),
                    new Claim("email_address", token_email_address!),
                    new Claim("email_verified", email_verified!),
                    new Claim("expiry_date", token_expiry!),
                };

                var finalJwtSecurityToken = new JwtSecurityToken(
                    clientId,
                    audience,
                    claimsForToken,
                    DateTime.UtcNow,
                    DateTime.UtcNow.AddHours(1));

                await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                    "authentication-controller@domain.local",
                    "admin@domain.local",
                    "User Token created",
                    $"Id Token created: {finalJwtSecurityToken}"
                ));

                return Ok(finalJwtSecurityToken);
            }                    

            return BadRequest(response.ReasonPhrase);
        }

        /// <summary>
        /// Method to attempt authentication for swagger
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate-swagger")]
        public async Task<IActionResult> AuthenticateSwaggerAsync()
        {
            var clientId = base._solutionSettings.Authentication.Schemes.Keycloak.ClientId;
            var clientSecret = base._solutionSettings.Authentication.Schemes.Keycloak.ClientSecret;
            var audience = base._solutionSettings.Authentication.Schemes.Keycloak.Audience;

            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.TokenExchange;
            var client = new HttpClient();

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("scope", "openid"),
                new KeyValuePair<string, string>("username", "swagger.user"),
                new KeyValuePair<string, string>("password", "swagger"),
            };
            var response = await client.PostAsync(endPoint, new FormUrlEncodedContent(data));

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<KeycloakResponse>(response.Content.ReadAsStringAsync().Result);

                await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                    "authentication-controller@domain.local",
                    "admin@domain.local",
                    "Swagger Token created",
                    $"Access Token created: {content?.access_token}"
                ));

                return Ok(content?.access_token);
            }

            return BadRequest(response.ReasonPhrase);
        }

        /// <summary>
        /// Get Property from Id Token
        /// </summary>
        /// <param name="jwtSecurityToken"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private Claim? GetClaimFromToken(JwtSecurityToken jwtSecurityToken, string propertyName)
        {
            return jwtSecurityToken.Claims.FirstOrDefault(c => c.Type!.Equals(propertyName));
        }
    }
}
