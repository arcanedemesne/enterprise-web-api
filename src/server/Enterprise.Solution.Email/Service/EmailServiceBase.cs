using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Extensions.Logging;

using Enterprise.Solution.Shared.Settings;

namespace Enterprise.Solution.Email.Service
{
    [ExcludeFromCodeCoverage]
    public abstract class EmailServiceBase : IEmailService
    {
        private readonly ILogger _logger;

        protected MailConfig MailConfig { get; } 

        protected EmailServiceBase(MailConfig mailConfig, ILogger logger)
        {
            MailConfig = mailConfig;
            _logger = logger;
        }

        // IS THIS NEEDED?
        protected bool RootCaValidationCallback(object sender, X509Certificate certificate, X509Chain certChain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                // If we come here that means the root CA is already trusted by the OS running this process
                // and there is no need to validate further.
                _logger.LogDebug("Machine already trusted the server CA. RootCa path is ignored. TLS Validation is successful");
                return true;  
            }
            
            _logger.LogDebug("Validation callback starts with SSL policy status set to = {Error}", sslPolicyErrors);

            var finalPath = Path.Combine(AppContext.BaseDirectory, MailConfig.CAPath!);
            _logger.LogDebug("Loading root CA Path with {FinalPath}", finalPath);
            if(!File.Exists(finalPath))
            {
                _logger.LogError("No Root CA is supplied thus validation of the chain can't be done");
                return false;
            }
            
            // Check if we have chain or just the cert, we're not allowing empty or just the cert
            if (certChain.ChainElements.Count < 2)
            {
                _logger.LogError("Remote certificate chain is missing, we only allowed server certificate signed by CA");
                return false;
            }

            // Load our root CA based on what is configured from the IOptions
            using var rootCa = new X509Certificate2(finalPath);
            _logger.LogDebug("Root CA is now loaded successfully, starting validation process");

            // Create a new chain for validation purposes that consist of the original chains and the root CA itself.
            // This class must be disposed when you're done
            // (https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.x509certificates.x509chain?view=netcore-3.1#remarks)
            // Since this is internal cert we don't have to worry about revocation
            using var newChain = new X509Chain();
            newChain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
            //newChain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllowUnknownCertificateAuthority;

            // SMTP server sometimes send the root CA cert, sometimes not which is a pain in the a$$. So we have to check if the first one 
            // on the chain is already the CA
            // https://www.rfc-editor.org/rfc/rfc5246#section-7.4.2
            _logger.LogDebug("Server has sent us {CertificateChainCount} certificate(s)", certChain.ChainElements.Count );
            for (var i = 1; i < certChain.ChainElements.Count; i++)
            {
                newChain.ChainPolicy.ExtraStore.Add(certChain.ChainElements[i].Certificate);
                _logger.LogDebug("Adding {CertificateName} as a chain", certChain.ChainElements[i].Certificate.Subject);
            }
            
            // Add our root CA (this potentially add same root CA twice, but ExtraStore doesn't really care about that
            newChain.ChainPolicy.ExtraStore.Add(rootCa);

            // Build our chain and check the status
            try
            {
                // Verify the chain of our leaf cert (the server certificate itself). The return value can not be trusted in this case
                // due to always false if there's even an untrusted root in the chain.
                // The expected result of the chain validation are the following:
                // 1. After build what the calculated chain state considering all the chain sent by the server,
                //    the last one must be our root CA...
                //    AND
                // 2. ChainStatus of the last one must be X509ChainStatusFlags.UntrustedRoot.
                
                // Verify our leaf cert if it is resulting in a good chain with the known intermediary CAs and Root CA
                var leafCert = certChain.ChainElements[0].Certificate;
                newChain.Build(leafCert);
                var lastItemInChain = newChain.ChainElements[^1];
                _logger.LogDebug("Last item in the computed chain is {@ChainStatus}", lastItemInChain.ChainElementStatus?.Select(x => x.Status));

                // Root CA validation
                // 1. Check if the content of config must match the root CA that comes from the calculated chain 
                var result = rootCa.GetRawCertData().SequenceEqual(lastItemInChain.Certificate.RawData);
                if (!result)
                {
                    _logger.LogError("The Root CA configured in the application can not be used to validate the server certificate chain");
                    return false;
                }
                
                // 2. Verify that either we don't have any bad status or the status is ONLY strictly Untrusted (because we implicitly trust them)
                if (lastItemInChain.ChainElementStatus == null)
                {
                    _logger.LogError("Invalid ChainElementStatus null is detected");
                    return false;
                }
                
                result &= (lastItemInChain.ChainElementStatus.Length == 0)  ||
                          lastItemInChain.ChainElementStatus.All(x => x.Status == X509ChainStatusFlags.UntrustedRoot);
                
                if (!result)
                {
                    _logger.LogError("The Root CA is invalid with {@Status}", lastItemInChain.ChainElementStatus.Select(x => x.Status));
                    return false;
                }
                
                _logger.LogInformation("TLS Validation with the configured root CA is successful");
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("The certificate that comes from the server can not be trusted with the root that is configured");
                return false;
            }
        }

        public virtual Task SendAsync(MailMessage message) => throw new NotImplementedException();
    }
}