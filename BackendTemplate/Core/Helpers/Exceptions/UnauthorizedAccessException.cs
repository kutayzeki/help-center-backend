using System.Net;

namespace FeedbackHub.Core.Helpers.Exceptions
{
    public class UnauthorizedAccessException : CustomException
    {
        public UnauthorizedAccessException(string message, List<string>? errors = default)
    : base(message, errors, HttpStatusCode.Unauthorized) { }
    }
}
