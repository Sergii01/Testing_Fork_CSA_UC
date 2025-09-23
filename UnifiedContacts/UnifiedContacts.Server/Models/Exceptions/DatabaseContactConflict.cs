using UnifiedContacts.Models.DbTables;

namespace UnifiedContacts.Models.Exceptions
{
    public class DatabaseContactConflict : Exception
    {
        public DatabaseContactDB ConflictingEntry { get; set; }
        public DatabaseContactConflict(DatabaseContactDB conflictEntry)
        {
            ConflictingEntry = conflictEntry;
        }

        public DatabaseContactConflict(DatabaseContactDB conflictEntry, string message)
            : base(message)
        {
            ConflictingEntry = conflictEntry;
        }

        public DatabaseContactConflict(DatabaseContactDB conflictEntry, string message, Exception inner)
            : base(message, inner)
        {
            ConflictingEntry = conflictEntry;
        }
    }
}
