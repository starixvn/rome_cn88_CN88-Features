namespace CN88.Features.Models
{
    using System.Net;
    using CN88.Features.Shared.AppResources;

    public class Result
    {
        public Result(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; }

        public string Message { get; }

        public static Result CreateSuccessResult(string message = "")
            => new Result(0, message);

        public static Result CreateErrorResult(int code = 1, string message = "")
            => new Result(code, message);

        public static Result CreateForbiddenResult()
            => new Result((int)HttpStatusCode.Forbidden, SharedResources.NotAuthorized);

        public bool IsSuccess => Code.Equals(0);
    }
}