using System.Security.Claims;
using UnifiedContacts.Models.Dto;
using UnifiedContacts.Repositories;

namespace UnifiedContacts.Engines
{
    public class TelemetryEngine
    {
        private readonly TelemetryRepository _telemetryRepository;
        private readonly RuntimeInfoDto _startupInfo;

        public TelemetryEngine(TelemetryRepository telemetryRepository, RuntimeInfoDto startupInfo)
        {
            _telemetryRepository = telemetryRepository;
            _startupInfo = startupInfo;
        }

        public async Task UpdateTelemetryHistoryData(UnifiedContactsSource source, int responseCount = 0, TeamsClientType? clientType = null)
        {
            if (!_startupInfo.DatabaseConfigured)
            {
                return;
            }

            if (!clientType.HasValue)
            {
                clientType = TeamsClientType.UNKNOWN;
            }

            List<Task> repositoryWriteTasks =
            [
                _telemetryRepository.RegisterRequestAsync(clientType.Value), // This is null checked above and inited if equal to null hence this can't be null here and .Value can be safely used here
                _telemetryRepository.RegisterResultsAsync(source, responseCount)
            ];

            await Task.WhenAll(repositoryWriteTasks.ToArray());
        }

        public Dictionary<string, string> CollectErrorTelemetryInfo(Exception e)
        {
            Dictionary<string, string> errorTelemetryInfo = new Dictionary<string, string>
            {
                { "errorMessage", e.Message },
                { "errorType", e.GetType().ToString() }
            };

            return errorTelemetryInfo;
        }

        public Dictionary<string, string> CollectCallerTelemetryInfo(ClaimsPrincipal user)
        {
            Dictionary<string, string> telemetryInfo = new Dictionary<string, string>
            {
                { "userId", user.GetObjectId() ?? "" },
                { "userDomain", user.GetObjectDomain() ?? "" },
                { "userTenantId", user.GetObjectTenantId() ?? "" }
            };

            return telemetryInfo;
        }
    }
}