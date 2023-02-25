using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Extensions.Logging;

using RazorEngine;
using RazorEngine.Templating;

namespace Enterprise.Solution.Email.Service
{
    /// <summary>
    /// Implementation service wrapper to RazorEngine Core (Net Core fork from RazorEngine)
    /// (https://github.com/fouadmess/RazorEngine)
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class HtmlContentGeneratorService : IHtmlContentGeneratorService
    {
        private readonly ILogger<HtmlContentGeneratorService> _logger;

        public HtmlContentGeneratorService(ILogger<HtmlContentGeneratorService> logger)
        {
            _logger = logger;
        }
        public Task<string> GenerateHtmlContentFromStringTemplate<T>(string templateContent, T model, CancellationToken token)
        {
            var templateKey = GetMd5Hash(templateContent);
            //bool isCached = false;
            //if (!string.IsNullOrEmpty(templateKey))
            //{
            //    // NOTE: Commenting the following line per Tristan's request due to issues with caching bad templates but no way to clear it at the moment.
            //    //       We are leaving the line just in case we want to add it back in the future when we have the ability to easily clear cache.
            //    // isCached = Engine.Razor.IsTemplateCached(templateKey, model.GetType());
            //}

            //var content = isCached ?
            //    Engine.Razor.Run(templateKey, model.GetType(), model) :
            //    Engine.Razor.RunCompile(templateContent, templateKey, model.GetType(), model);
            var content = Engine.Razor.RunCompile(templateContent, templateKey, model?.GetType(), model);

            _logger.LogDebug(
                $"HtmlContentGeneratorService has generated template with model={model?.GetType()} " +
                $"content={content}");

            return Task.FromResult(content);
        }

        private static string GetMd5Hash(string input)
        {
            var md5 = MD5.Create();
            var inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (byte t in hash)
            {
                sb.Append(t.ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
