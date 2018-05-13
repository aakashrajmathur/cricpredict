using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace cricpredict.Controllers
{
    public class EmailActions
    {
        private string api = "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";
        private static string FromEmail = "suggestion@cricpredict.online";
        private static string User = "Suggestion - CricPredict";
        private static string EmailSubject = "Suggestions & Comments page recieved an input";

        public async Task<bool> SendEmailsAsync(string htmlContent)
        {
            try
            {
                await SendEmail("aakashrajmathur@gmail.com", htmlContent);
                //SendEmail("ursdevesh@gmail.com");
                return true;
            }
            catch {
                return false;
            }
        }

        private async Task<bool> SendEmail(string toEmail, string htmlContent)
        {
            var client = new SendGridClient(api);
            var from = new EmailAddress(FromEmail, User);
            var subject = EmailSubject;
            var to = new EmailAddress(toEmail, User);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            return (response.StatusCode == System.Net.HttpStatusCode.Accepted);
        }
    }
}