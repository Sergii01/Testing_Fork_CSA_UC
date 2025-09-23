using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.Data.SqlClient;
using UnifiedContacts.Models.DbTables;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Settings;

namespace UnifiedContacts.Repositories
{
    public class DatabaseContactsRepository : RepositoryBase
    {
        private readonly AuthSettings _authSettings;
        public DatabaseContactsRepository(AuthSettings authSettings, RuntimeInfoDto startupInfo) : base(startupInfo)
        {
            _authSettings = authSettings;
        }

        private static string GetPhoneNumberFormattingSQL(string parameterName)
        {
            return $"REPLACE(TRANSLATE(REPLACE({parameterName}, '(0)', ''), '()+-/', '     '), ' ', '')";
        }

        public async Task<int> GetContactCountAsync()
        {
            VerifyDatabaseConfiguration();

            string sql = $"SELECT COUNT(*) FROM [UnifiedContactsCustom].[Contacts]";

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<IEnumerable<DatabaseContactDB>> SearchDatabaseContactsByPhoneNumberAsync(string phoneNumber)
        {
            VerifyDatabaseConfiguration();

            string phoneSearchValue = $"%{phoneNumber.Replace(" ", string.Empty).Replace("+", string.Empty)}%";

            string sql = $"SELECT * FROM [UnifiedContactsCustom].[Contacts] WHERE {GetPhoneNumberFormattingSQL("mobilePhoneNumbers")} LIKE @number OR {GetPhoneNumberFormattingSQL("businessPhoneNumbers")} LIKE @number OR {GetPhoneNumberFormattingSQL("homePhoneNumbers")} LIKE @number";

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.QueryAsync<DatabaseContactDB>(sql, new { number = $"{phoneSearchValue}" });
            }
        }

        public async Task<IEnumerable<DatabaseContactDB>> SearchDatabaseContactsAsync(string query)
        {
            VerifyDatabaseConfiguration();

            string sql = $"SELECT TOP(100) * FROM [UnifiedContactsCustom].[Contacts] WHERE displayName LIKE @search OR jobTitle LIKE @search OR department LIKE @search OR companyName LIKE @search OR mailAddresses LIKE @search OR imAddresses LIKE @search OR mobilePhoneNumbers LIKE @search OR businessPhoneNumbers LIKE @search OR homePhoneNumbers LIKE @search OR addressFullString LIKE @search OR addressStreetAddress LIKE @search OR addressPostalCode LIKE @search OR addressCity LIKE @search OR addressCountry LIKE @search";
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.QueryAsync<DatabaseContactDB>(sql, new { search = $"%{query.Trim().Replace("%", string.Empty)}%" });
            }
        }

        public async Task<IEnumerable<DatabaseContactDB>> GetDatabaseContactsByIdsAsync(IEnumerable<string> ids)
        {
            VerifyDatabaseConfiguration();

            string sql = "SELECT * FROM [UnifiedContactsCustom].[Contacts] WHERE id IN @ids";
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.QueryAsync<DatabaseContactDB>(sql, new { ids = ids });
            }
        }

        public async Task<DatabaseContactDB> GetDatabaseContactsByIdAsync(string id)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                return await connection.GetAsync<DatabaseContactDB>(id);
            }
        }

        public async Task InsertDatabaseContactAsync(DatabaseContactDB contact)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.InsertAsync(contact);
            }
        }

        public async Task UpdateDatabaseContactAsync(DatabaseContactDB contact)
        {
            VerifyDatabaseConfiguration();

            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.UpdateAsync(contact);
            }
        }

        public async Task DeleteDatabaseContactAsync(string contactId)
        {
            VerifyDatabaseConfiguration();

            string sql = "DELETE FROM [UnifiedContactsCustom].[Contacts] WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(_authSettings.DatabaseConnectionString))
            {
                await connection.QueryAsync<DatabaseContactDB>(sql, new { id = contactId });
            }
        }


    }
}
