namespace UnifiedContacts.Server.Models.Exceptions.EntraIdFilter
{
    public class EntraIdFilterLimitExceededException : Exception
    {
        public EntraIdFilterLimitExceededException()
        {
        }

        public EntraIdFilterLimitExceededException(string? message) : base(message)
        {
        }

        public EntraIdFilterLimitExceededException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
