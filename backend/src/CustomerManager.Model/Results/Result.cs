using System;
using System.Net;

namespace CustomerManager.Models.Results
{
    public class Result
    {
        public HttpStatusCode StatusCode { get; set; }
        public Exception Error { get; set; }

        public Result(HttpStatusCode statusCode, Exception error = null)
        {
            StatusCode = statusCode;
            Error = error;
        }
    }

    public class Result<T> : Result
    {
        public T Value { get; set; }

        public Result(T value, HttpStatusCode statusCode, Exception error = null) : base(statusCode, error)
        {
            Value = value;
        }
    }
}