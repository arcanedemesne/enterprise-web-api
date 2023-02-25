using System.Diagnostics.CodeAnalysis;

namespace Enterprise.Solution.API.Configuration
{
    [ExcludeFromCodeCoverage]
    internal static class AppSettingsConfig
    {
        private const string EnvironmentVariablePrefix = "SolutionApi_";

        internal static void ConfigureAppSettings(IConfigurationBuilder configBuilder)
        {
            // By default, all settings will be loaded first from [appsettings.json],
            // and then updated by [appsettings.{ASPNETCORE_ENVIRONMENT}.json] if available

            configBuilder.AddEnvironmentVariables(EnvironmentVariablePrefix);

            // Any app settings defined in Environment Variables (usually via the container) will override the appsettings.json file.
            // Environment variables must be prefixed followed by the key.
            // Setting hierarchy is denoted with a colon (:) or a double-underscore (__) between each level.
            // So the connection string, as an environment variable, should be named something like this:
            //   "SolutionApi_SolutionSettings::Database::Host"
            // or this:
            //   "SolutionApi_SolutionSettings__Database__Host"
        }
    }

}
