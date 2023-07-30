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
        public Keycloak Keycloak { get; set; } = null!;
    }

    [ExcludeFromCodeCoverage]
    public class Keycloak
    {
        public string Audience { get; set; } = null!;
        public string Authority { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string MetadataAddress { get; set; } = null!;
        public string TokenExchange { get; set; } = null!;
        public string Users { get; set; } = null!;
    }
}
