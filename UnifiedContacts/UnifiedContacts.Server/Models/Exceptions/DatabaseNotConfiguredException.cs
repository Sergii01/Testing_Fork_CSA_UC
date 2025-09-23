namespace UnifiedContacts.Models.Exceptions
{
    public class DatabaseNotConfiguredException : Exception
    {
        public DatabaseNotConfiguredException()
        {
        }

        public DatabaseNotConfiguredException(string message)
            : base(message)
        {
        }

        public DatabaseNotConfiguredException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
