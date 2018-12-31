using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeriDavay.Website.Models
{
    public class SendGridSettings
    {
        public string SendGridApiKey { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
