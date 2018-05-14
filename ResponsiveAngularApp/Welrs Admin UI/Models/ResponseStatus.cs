using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminUI.Models
{
    public class ResponseStatus
    {
        public enum ApiResponseStatus { Success = 1, Failure = 0 };

        public ApiResponseStatus Success { get; set; } = 0;
        public String ErrorMessage { get; set; } = "";
    }

}