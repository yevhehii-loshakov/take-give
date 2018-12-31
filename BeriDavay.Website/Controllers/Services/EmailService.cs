using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BeriDavay.Website.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BeriDavay.Website.Controllers.Services
{
    public class EmailService
    {

        private readonly SendGridSettings _sendGridConfig;

        public EmailService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridConfig = sendGridSettings.Value;
        }

        public static async Task<BaseResult> SendEmail(string email, string subject, string emailTemplate)
        {
            var result = new BaseResult();
            //ToDo: Template logic!!!
            MailAddress addr;
            try
            {
                addr = new MailAddress(email);
            }
            catch (Exception e)
            {
                result.ResultStatus = false;

                var error = new BaseError()
                {
                    StatusCode = null,
                    ErrorDescription = "Faild Email address!",
                    ExceptionMessage = e.Message
                };

                result.Errors.Add(error);

                return result;
            }

            var status = await SendEmailCore(email, subject, emailTemplate);

            if (status != HttpStatusCode.Accepted)
            {
                result.ResultStatus = false;

                var error = new BaseError
                {
                    StatusCode = status,
                    ErrorDescription = "Check SendEmailCore, Send fails",
                    ExceptionMessage = null
                };
                result.Errors.Add(error);

                return result;
            }

            return new BaseResult { ResultStatus = true };
        }




        private async Task<HttpStatusCode> SendEmailCore(string email, string subject, string htmlContent)
        {
            var apiKey = _sendGridConfig.SendGridApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("permit@beridabay.org", subject);
            var to = new EmailAddress(email);
            var plainTextContent = Regex.Replace(htmlContent, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            return response.StatusCode;
        }

    }
}
