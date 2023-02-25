using System.Net.Mail;

namespace Enterprise.Solution.Email.Service
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends Specified Email
        /// </summary>
        /// <param name="message">The email to send</param>
        /// <returns></returns>
        Task SendAsync(MailMessage message);
    }
}
