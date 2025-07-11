using System.Net;

namespace GenericApi.Constants
{
    public static class ErrorMessages
    {
        public static readonly Dictionary<HttpStatusCode, string> ErrorText = new()
        {
            { HttpStatusCode.InternalServerError, "Internal server error" },
            { HttpStatusCode.NotFound, "Could not find order" },
            { HttpStatusCode.UnprocessableContent, "Id argument must be empty in request body" },
            { HttpStatusCode.BadRequest, "Invalid request body" },
        };
    }
}
