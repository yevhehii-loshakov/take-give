using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeriDavay.Website.Models
{
    public class BaseResult
    {
        public bool ResultStatus { get; set; }
        public List<BaseError> Errors { get; set; }
        


        public BaseResult()
        {
            Errors = new List<BaseError>();
        }
    }

    public class BaseError
    {
        public string ErrorDescription { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
    }
}
