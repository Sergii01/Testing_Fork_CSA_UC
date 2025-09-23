namespace UnifiedContacts.Server.Models.Exceptions.EntraIdFilter
{
    public class EntraIdFilterNotFoundException : Exception
    {
        public EntraIdFilterNotFoundException()
        {
        }

        public EntraIdFilterNotFoundException(string? message) : base(message)
        {
        }

        public EntraIdFilterNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
