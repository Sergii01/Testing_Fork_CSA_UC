namespace UnifiedContacts.Models.Exceptions
{
    public class DatabaseContactNotFound : Exception
    {
        public DatabaseContactNotFound()
        {
        }

        public DatabaseContactNotFound(string message)
            : base(message)
        {
        }

        public DatabaseContactNotFound(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
