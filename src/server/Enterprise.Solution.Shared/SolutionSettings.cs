using System.Diagnostics.CodeAnalysis;
using Enterprise.Solution.Shared.Settings;

namespace Enterprise.Solution.Shared
{
    [ExcludeFromCodeCoverage]
    public class SolutionSettings
    {
        public AuthenticationConfig Authentication { get; set; } = new AuthenticationConfig();

        public DatabaseConfig Database { get; set; } = new DatabaseConfig();

        public CacheConfig Cache { get; set; } = new CacheConfig();

        public MailConfig MailSettings { get; set; } = new MailConfig();

        // The user-facing application URL scheme
        public string FrontEndApplicationScheme { get; set; } = null!;

        // The user-facing application URL domain and port
        public string FrontEndApplicationAuthority { get; set; } = null!;

        /// <summary>
        /// This tenant id only used for localhost since there's no easy way to set tenant info for local development.
        /// In the live configuration, K8s ingress will have a specific tenancy information
        /// </summary>
        public string TenantId { get; set; } = null!;

        public CookieSettings CookieSettings { get; set; } = new CookieSettings();

        public string EncryptionKeyBase64 { get; set; } = null!;
    }
}
