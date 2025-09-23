namespace UnifiedContacts.Server.Models.Exceptions
{
    public class HttpResponseException : Exception
    {
        public readonly HttpResponseMessage Response;

        public HttpResponseException(HttpResponseMessage response, string? message = null) : base(message)
        {
            Response = response;
        }

    }
}
