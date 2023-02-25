using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.Shared.Settings
{
    [ExcludeFromCodeCoverage]
    public class CacheConfig
    {
        public string Host { get; set; } = null!;
        public ushort Port { get; set; }
        public bool AbortConnect {  get; set; }
        public int ConnectTimeout { get; set; }
        public int ResponseTimeout { get; set; }

        /// <summary>
        /// Returns a formatted redis connection string based on the configuration properties
        /// </summary>
        public string GetConnectionString()
        {
            var connString =
                $"{Host}:{Port},abortConnect={AbortConnect},connectTimeout={ConnectTimeout},responseTimeout={ResponseTimeout}";
            return connString;
        }
    }
}
