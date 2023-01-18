using System.Net;

namespace FeedbackHub.Core.Helpers.Exceptions
{
    public class InternalServerException : CustomException
    {
        public InternalServerException(string message, List<string>? errors = default)
            : base(message, errors, HttpStatusCode.InternalServerError) { }
    }
}
