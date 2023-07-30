using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Enterprise.Solution.Shared;

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
            public string? UserName { get; set; }
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
        public async Task<IActionResult> AuthenticateAsync()
        {
            Request.Headers.TryGetValue("X-UserName", out var UserName);
            Request.Headers.TryGetValue("X-Password", out var Password);

            var response = await Authenticate(UserName!, Password!);

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<KeycloakResponse>(response.Content.ReadAsStringAsync().Result);

                var id_token = content?.id_token;
                JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(id_token);

                var token_username = GetClaimFromToken(jwtSecurityToken, "preferred_username")?.Value;
                var token_given_name = GetClaimFromToken(jwtSecurityToken, "given_name")?.Value;
                var token_family_name = GetClaimFromToken(jwtSecurityToken, "family_name")?.Value;
                var token_email_address = GetClaimFromToken(jwtSecurityToken, "email")?.Value;
                var token_user_role = GetClaimFromToken(jwtSecurityToken, "aud")?.Value;
                var token_keycloak_uid = GetClaimFromToken(jwtSecurityToken, "sub")?.Value;

                Guid userGuid;
                if (Guid.TryParse(token_keycloak_uid, out userGuid)) {
                    if (await UserService.ExistsAsync(userGuid))
                    {
                        // TODO: Update User Info
                        var User = await UserService.GetByIdAsync(userGuid);
                        if (User != null)
                        {
                            User.UserName = token_username!;
                            User.FirstName = token_given_name!;
                            User.LastName = token_family_name!;
                            User.EmailAddress = token_email_address!;

                            User.ModifiedBy = userGuid;
                            User.ModifiedTs = DateTime.UtcNow;

                            await UserService.UpdateAsync(User);
                        }
                    }
                    else
                    {
                        // TODO: Create User Info
                        var User = await UserService.AddAsync(new Data.Models.User()
                        {
                            KeycloakUniqueIdentifier = userGuid,
                            UserName = token_username!,
                            FirstName = token_given_name!,
                            LastName = token_family_name!,
                            EmailAddress = token_email_address!,

                            CreatedBy = userGuid,
                        });
                    }

                    await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                        "authentication-controller@domain.local",
                        "admin@domain.local",
                        $"Token created for User: {token_username} - with KeyclaokId: {token_keycloak_uid}",
                        $"Id Token created: {content}"
                    ));

                    return Ok(content);
                }

                return BadRequest("Invalid User");
            }                    

            return BadRequest(response.ReasonPhrase);
        }

        #if DEBUG
        /// <summary>
        /// Method to attempt authentication for swagger
        /// </summary>
        /// <returns></returns>
        [HttpPost("authenticate-swagger")]
        public async Task<IActionResult> AuthenticateSwaggerAsync()
        {
            var response = await Authenticate("swagger.user", "swagger");

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<KeycloakResponse>(response.Content.ReadAsStringAsync().Result);

                await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                    "authentication-controller@domain.local",
                    "admin@domain.local",
                    "Swagger Token created",
                    $"Swagger Token created: {content?.access_token}"
                ));

                return Ok(content?.access_token);
            }

            return BadRequest(response.ReasonPhrase);
        }
        #endif
        
        /// <summary>
        /// Method to attempt authentication for swagger
        /// </summary>
        /// <returns></returns>
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RequestAccessTokenAsync()
        {
            Request.Headers.TryGetValue("X-Refresh-Token", out var token);

            var response = await Refresh(token!);

            if (response.IsSuccessStatusCode)
            {
                var content = JsonConvert.DeserializeObject<KeycloakResponse>(response.Content.ReadAsStringAsync().Result);

                await EmailService.SendAsync(new System.Net.Mail.MailMessage(
                    "authentication-controller@domain.local",
                    "admin@domain.local",
                    "User Token refreshed",
                    $"Refresh Token created: {content?.refresh_token}"
                ));

                return Ok(content);
            }

            return BadRequest(response.ReasonPhrase);
        }

        private async Task<HttpResponseMessage> Authenticate(string UserName, string Password)
        {

            var clientId = base._solutionSettings.Authentication.Schemes.Keycloak.ClientId;
            var clientSecret = base._solutionSettings.Authentication.Schemes.Keycloak.ClientSecret;

            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.TokenExchange;
            var client = new HttpClient();

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("scope", "openid"),
                new KeyValuePair<string, string>("username", UserName),
                new KeyValuePair<string, string>("password", Password),
            };

            return await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
        }

        private async Task<HttpResponseMessage> Refresh(string refresh_token)
        {

            var clientId = base._solutionSettings.Authentication.Schemes.Keycloak.ClientId;
            var clientSecret = base._solutionSettings.Authentication.Schemes.Keycloak.ClientSecret;

            string endPoint = base._solutionSettings.Authentication.Schemes.Keycloak.TokenExchange;
            var client = new HttpClient();

            var data = new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("refresh_token", refresh_token),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
            };
        
            return await client.PostAsync(endPoint, new FormUrlEncodedContent(data));
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
