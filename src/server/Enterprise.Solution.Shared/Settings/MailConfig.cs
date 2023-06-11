using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.Shared.Settings
{
    /// <summary>
    /// Configuration object for mail service
    /// </summary>
    /// <example>
    ///<code>
    /// <para/>// Define settings as this:
    /// <para/>Host = localhost;
    /// <para/>Port = 9025; // 9025 for stunnel, 1025 for mailhog
    /// <para/>UseCredentials = true; // use false to use defined username and password instead
    /// <para/>Password = "password value"; // used when UseCredentials is false
    /// <para/>Username = "username value"; // used when UseCredentials is false
    /// <para/>UseSsl = true; // true is expected for stunnel and false for mailhog
    /// <para/>CertificateString = "certificate string value"; // string contained in .pem file in one line format.
    /// </code>
    /// </example>
    [ExcludeFromCodeCoverage]
    public class MailConfig
    {
        public enum MailProvider { Default = 0, MailKit}
        public string Host { get; set; } = null!;
        public ushort Port { get; set; }
        public bool UseAuthentication { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public bool UseSsl { get; set; }
        /// <summary>
        /// Specify the path of the root CA relative to the application path in crt/pem format.
        /// Require if UseSsl is true.
        /// </summary>
        public string? CAPath { get; set; }
        public MailProvider Provider { get; set; } = MailProvider.Default;

        /// <summary>
        /// This settings control if we're doing explicit TLS or sometimes referred to smtp with STARTTLS
        /// vs. implicit TLS also sometimes referred to as mutual TLS.
        /// Default is false, aka. STARTTLS mode 
        /// </summary>
        public bool UseMutualTls { get; set; } = false;
    }
}
