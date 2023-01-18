using System.Net;

namespace FeedbackHub.Core.Helpers.Exceptions
{
    public class BadRequestException : CustomException
    {
        public BadRequestException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.BadRequest) { }
    }
}
