using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Neowrk.Library.Rest
{
    public class ApiResponse
    {
        public ApiResponse()
        {
            Success = false;
            Messages = new List<string>();
            Fields = new List<KeyValuePair<string, string>>();
        }

        public bool Success { get; private set; }
        public int Code { get; private set; }
        public List<string> Messages { get; private set; }
        public List<KeyValuePair<string, string>> Fields { get; private set; }
        public object Data { get; set; }

        public void SuccessResponse(string message)
        {
            Messages.Add(message);
            Success = true;
            Code = (int)HttpStatusCode.OK;
        }

        public void ErrorResponse(string message)
        {
            Messages.Add(message);
            Code = (int)HttpStatusCode.BadRequest;
        }

        public void ErrorResponse(IEnumerable<string> message)
        {
            Messages.AddRange(message);
            Code = (int)HttpStatusCode.BadRequest;
        }

        public void BadRequestResponse()
        {
            Code = (int)HttpStatusCode.BadRequest;
        }
    }
}
