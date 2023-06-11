using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.Shared.Settings
{
    [ExcludeFromCodeCoverage]
    public class AuthenticationConfig
    {
        public string DefaultScheme { get; set; } = null!;
        public Schemes Schemes { get; set; } = null!;
    }

    [ExcludeFromCodeCoverage]
    public class Schemes
    {
        public Swagger Swagger { get; set; } = null!;
    }

    [ExcludeFromCodeCoverage]
    public class Swagger
    {
        public string Audience { get; set; } = null!;
        public string ClaimsIssuer { get; set; } = null!;
        public string SecretForKey { get; set; } = null!;
    }

    [ExcludeFromCodeCoverage]
    public class Keycloak
    {
        public string ServerRealm { get; set; } = null!;
        public string Metadata { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string TokenExchange { get; set; } = null!;
        public string Audience { get; set; } = null!;
    }
}
