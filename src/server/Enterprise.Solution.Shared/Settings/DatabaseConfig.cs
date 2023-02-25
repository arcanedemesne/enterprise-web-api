using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.Shared.Settings
{
    [ExcludeFromCodeCoverage]
    public class DatabaseConfig
    {
        public string Host { get; set; } = null!;
        public ushort Port { get; set; }
        public string Database { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool RequireSsl { get; set; }

        /// <summary>
        /// Returns a formatted postgres connection string based on the configuration properties
        /// </summary>
        public string GetConnectionString()
        {
            return
                $"Server={Host};" +
                $"Port={Port};" +
                $"Database={Database};" +
                $"User Id={Username};" +
                $"Password={Password};" +
                $"Ssl Mode={(RequireSsl ? "Require" : "Prefer")};";
        }
    }
}
