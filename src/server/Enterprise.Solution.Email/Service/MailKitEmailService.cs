using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;

using Enterprise.Solution.Shared;

using MailKit;
using MailKit.Security;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MimeKit;

using SmtpClient = MailKit.Net.Smtp.SmtpClient;  // SmtpClient is set to use MailKit

namespace Enterprise.Solution.Email.Service
{
    /// <summary>
    /// MailKit based implementation of EmailService. The background of the necessity of this driven
    /// from official docs of SmtpClient that is under deprecation. The recommend MailKit instead.
    /// https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=net-5.0#remarks 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MailKitEmailService : EmailServiceBase
    {
        private readonly ILogger<MailKitEmailService> _logger;

        public MailKitEmailService(IOptions<SolutionSettings> settings, ILogger<MailKitEmailService> logger) 
            : base(settings.Value.MailSettings, logger)
        {
            _logger = logger;
            _logger.LogDebug("System is configured to use Mailkit based email service");
        }
        public override async Task SendAsync(MailMessage message)
        {
            ValidateConfig();
            
            using var smtpClient = new SmtpClient()
            {
                CheckCertificateRevocation = false,
                Timeout = 10000, // 10 seconds timeout
            };
            try
            {
                // How nice are they to implement an explicit cast operator from .NET Core MailMessage
                // http://www.mimekit.net/docs/html/M_MimeKit_MimeMessage_op_Explicit.htm
                var mimeMessage = (MimeMessage)message;
                var secureSocketOptions = SecureSocketOptions.None;
                if (MailConfig.UseSsl && !string.IsNullOrEmpty(MailConfig.CAPath))
                {
                    secureSocketOptions = MailConfig.UseMutualTls ?
                        SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                    smtpClient.ServerCertificateValidationCallback = RootCaValidationCallback!;
                }
                await smtpClient.ConnectAsync(MailConfig.Host, MailConfig.Port, secureSocketOptions);

                if(MailConfig.UseAuthentication)
                    await smtpClient.AuthenticateAsync(MailConfig.Username, MailConfig.Password);

                // Send
                _logger.LogDebug("Invoking SendAsync with configuration value of {@MailConfig}", MailConfig);
                await smtpClient.SendAsync(mimeMessage);
            }
            catch (ServiceNotAuthenticatedException notAuthenticated)
            {
                _logger.LogError("EmailService has failed to authenticate. Check the credential used by the system");
                throw new InvalidOperationException(notAuthenticated.Message, notAuthenticated);
            }
            catch (ServiceNotConnectedException connectionException)
            {
                // Use structured logging syntax
                _logger.LogError("EmailService has failed to establish connection to the target smtp server {TargetEmailServer}:{Port}", MailConfig.Host, MailConfig.Port);
                throw new InvalidOperationException(connectionException.Message, connectionException);
            }
            catch (Exception e)
            {
                _logger.LogError("EmailService has encounter an error at the SMTP client level. Please check the inner exception to get more detail exception");
                throw new InvalidOperationException(e.Message, e);
            }
            finally
            {
                // Ensure disconnection no matter what
                await smtpClient.DisconnectAsync(true);
            }
        }

        private void ValidateConfig()
        {
            // Check host and port
            if (string.IsNullOrEmpty(MailConfig.Host))
            {
                throw new InvalidOperationException("SMTP host and port are required. Check your settings");
            }
            
            // Check if authentication is on we have username password
            if (MailConfig.UseAuthentication)
            {
                if (string.IsNullOrEmpty(MailConfig.Username) || string.IsNullOrEmpty(MailConfig.Password))
                {
                    throw new InvalidOperationException("SMTP with authentication enable you must specify username and password");
                }
            }
        }
    }
}