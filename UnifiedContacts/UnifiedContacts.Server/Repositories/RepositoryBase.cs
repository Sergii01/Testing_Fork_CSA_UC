using UnifiedContacts.Models.Dto;
using UnifiedContacts.Models.Exceptions;

namespace UnifiedContacts.Repositories
{
    public abstract class RepositoryBase
    {
        private readonly RuntimeInfoDto _startupInfoDto;
        protected RepositoryBase(RuntimeInfoDto startupInfoDto)
        {
            _startupInfoDto = startupInfoDto;
        }

        /// <summary>
        /// This function throws an exception if the Database is not configured properly
        /// </summary>
        /// <exception cref="DatabaseNotConfiguredException"></exception>
        protected void VerifyDatabaseConfiguration()
        {
            if (!_startupInfoDto.DatabaseConfigured)
            {
                throw new DatabaseNotConfiguredException("Database not configured");
            }
        }

        protected bool IsDatabaseConfigured()
        {
            return _startupInfoDto.DatabaseConfigured;
        }
    }
}
