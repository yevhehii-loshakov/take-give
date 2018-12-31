using System;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BeriDavay.Website.Controllers.Services;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Send mesage 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="takeGive"> true if "Give"</param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("SendPermitEmail")]
        public async Task<IActionResult> SendPermitEmail(string email, string requesterName, bool takeGive = true)
        {
            string subject = takeGive ? "Some one gave you permit!" : "Some one ask you for permit!";

            string emailTemplate = "Bla bla bla"; //Todo: add template

            var result = await EmailService.SendEmail(email, subject, emailTemplate);

            if (!result.ResultStatus)
            {
                return BadRequest(new {erros = result.Errors});
            }

            return Ok("User was notified");
        }





        
    }
}