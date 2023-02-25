using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Enterprise.Solution.Shared;

namespace Enterprise.Solution.Email.Service
{
    [ExcludeFromCodeCoverage]
    public class EmailService : EmailServiceBase
    {
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SolutionSettings> settings, ILogger<EmailService> logger) : base(settings.Value.MailSettings, logger)
        {
            _logger = logger;
        }

        public override async Task SendAsync(MailMessage message)
        {
            var smtpClient = InitSmtpClient();
            _logger.LogDebug("Invoking SendAsync with configuration value of {@MailConfig}", MailConfig);
            await smtpClient.SendMailAsync(message);
        }

        private SmtpClient InitSmtpClient()
        {
            var smtpClient = new SmtpClient();
            smtpClient.Host = MailConfig.Host != string.Empty ? MailConfig.Host : "localhost";
            smtpClient.Port = MailConfig.Port;
            smtpClient.UseDefaultCredentials = false; // We never run under server creds
            if (MailConfig.UseAuthentication)
            {
                if (string.IsNullOrWhiteSpace(MailConfig.Username) || string.IsNullOrWhiteSpace(MailConfig.Password))
                    _logger.LogError($"EmailService: Username or password are empty");
                else
                {
                    smtpClient.Credentials = new System.Net.NetworkCredential(MailConfig.Username, MailConfig.Password);
                }
            }

            smtpClient.EnableSsl = MailConfig.UseSsl;
            switch (MailConfig.UseSsl)
            {
                case true when MailConfig.UseMutualTls:
                    throw new NotSupportedException("This IEmailService implementation does not support Mutual TLS.");
                case true when !string.IsNullOrEmpty(MailConfig.CAPath):
                    // Disabling certificate validation can expose you to a man-in-the-middle attack
                    // which may allow your encrypted message to be read by an attacker
                    // https://stackoverflow.com/a/14907718/740639
                    ServicePointManager.ServerCertificateValidationCallback = RootCaValidationCallback!;
                    break;
            }

            return smtpClient;
        }
    }
}
