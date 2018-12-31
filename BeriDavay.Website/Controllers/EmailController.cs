using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using BeriDavay.Website.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace BeriDavay.Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        private readonly SendGridSettings _sendGridConfig;

        public EmailController(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridConfig = sendGridSettings.Value;
        }

        /// <summary>
        /// Send mesage 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="takeGive"> true if "Give"</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> SendPermitEmail(string email, string requesterName, bool takeGive = true)
        {
            string subject = takeGive ? "Some one gave you permit!" : "Some one ask you for permit!";

            var result = await SendEmail(email, subject);

            if (!result.ResultStatus)
            {
                return BadRequest(new {erros = result.Errors});
            }

            return Ok("User was notified");
        }





        private async Task<BaseResult> SendEmail(string email, string subject)
        {
            var result = new BaseResult();

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

            var status = await SendEmailCore(email, "Test", "Test!!!");

            if (status != HttpStatusCode.Accepted)
            {
                result.ResultStatus = false;

                var error = new BaseError()
                {
                    StatusCode = status,
                    ErrorDescription = "Check SendEmailCore, Send fails",
                    ExceptionMessage = null
                };
                result.Errors.Add(error);

                return result;
            }

            return new BaseResult(){ResultStatus = true};
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