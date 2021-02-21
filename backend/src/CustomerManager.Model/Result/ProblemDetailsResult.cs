using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace CustomerManager.Model.Result
{
    public class ProblemDetailsResult
    {
        private readonly ProblemDetails _problemDetails;

        public ProblemDetailsResult(HttpStatusCode statusCode, HttpRequest req, string detail)
        {
            _problemDetails = new ProblemDetails()
            {
                Title = statusCode.ToString(),
                Instance = req.HttpContext.Request.Path,
                Detail = detail.Replace($"{Environment.NewLine}", " "),
                Status = (int)statusCode,
                Type = "about:blank"
            };
        }

        public ProblemDetailsResult(HttpStatusCode statusCode, HttpRequest req, Exception ex)
        {
            _problemDetails = new ProblemDetails()
            {
                Title = statusCode.ToString(),
                Instance = req.HttpContext.Request.Path,
                Detail = ex.Message.Replace($"{Environment.NewLine}", " "),
                Status = (int)statusCode,
                Type = "about:blank"
            };
        }

        public ObjectResult GetObjectResult()
        {
            return new ObjectResult(_problemDetails) { StatusCode = _problemDetails.Status };
        }
    }
}
