using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.Shared.Settings
{
    /// <summary>
    /// Settings to control how cookie is generated
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class CookieSettings
    {
        public CookieSettings()
        {
            // Default cookie policy
            HttpOnly = true;
            Secure = true;
        }

        /// <summary>
        /// Set if https only is set, default is true
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>
        /// Control if cookie setting HttpOnly is set, default is true
        /// </summary>
        public bool HttpOnly { get; set; }
    }
}
