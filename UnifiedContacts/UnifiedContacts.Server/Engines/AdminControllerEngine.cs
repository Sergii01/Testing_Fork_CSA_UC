using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using System.Text.Json;
using UnifiedContacts.Interfaces;
using UnifiedContacts.Models.Responses.Admin;
using UnifiedContacts.Repositories;
using UnifiedContacts.Server.Models.DbTables;
using UnifiedContacts.Server.Models.Dto;
using UnifiedContacts.Server.Models.Exceptions.EntraIdFilter;
using UnifiedContacts.Server.Models.Payloads;

namespace UnifiedContacts.Engines
{
    public class AdminControllerEngine
    {
        private readonly DatabaseContactsRepository _databaseContactsRepository;
        private readonly TelemetryRepository _telemetryRepository;
        private readonly SettingsRepository _settingsRepository;
        private readonly IGraphApiEngine _graphApiEngine;

        public AdminControllerEngine(DatabaseContactsRepository databaseContactsRepository, TelemetryRepository telemetryEngine, SettingsRepository settingsRepository, IGraphApiEngine graphApiEngine)
        {
            _databaseContactsRepository = databaseContactsRepository;
            _telemetryRepository = telemetryEngine;
            _settingsRepository = settingsRepository;
            _graphApiEngine = graphApiEngine;
        }

        public async Task<GetMetricsResponse> GetMetricsAsync()
        {
            List<Metric> allMetrics = new();

            // Contacts in DB Metric
            string descriptionContactCountMetric = "Database Contacts";
            int contactCount = await _databaseContactsRepository.GetContactCountAsync();
            Metric contactCountMetric = new DisplayNumberMetric(descriptionContactCountMetric, contactCount);
            allMetrics.Add(contactCountMetric);

            //active users
            string descriptionActiveUserCountMetric = "Active App User";
            int activeUserCount = await _telemetryRepository.GetLast30DaysActiveUserCount();
            Metric activeCountMetric = new DisplayNumberMetric(descriptionActiveUserCountMetric, activeUserCount);
            allMetrics.Add(activeCountMetric);

            // Favorites usage
            string descriptionMaxFavoriteMetric = "Most favorites of user";
            int maxFavoriteCount = await _telemetryRepository.GetMaxFavoritesOfUser();
            DisplayNumberMetric maxFavoriteMetric = new(descriptionMaxFavoriteMetric, maxFavoriteCount);
            allMetrics.Add(maxFavoriteMetric);

            // Average Favorites usage
            string descriptionAvgFavoriteMetric = "Average favorites of users";
            int avgFavoriteCount = await _telemetryRepository.GetAverageFavoriteCountOfUsers();
            DisplayNumberMetric avgFavoriteMetric = new(descriptionAvgFavoriteMetric, avgFavoriteCount);
            allMetrics.Add(avgFavoriteMetric);

            return new GetMetricsResponse()
            {
                Metrics = allMetrics
            };
        }

        public async Task UpdateEntraIdFilter(Guid filterId, EntraIdFilterPayload filter)
        {
            string? currentFilterAttributesAsJsonString = await _settingsRepository.GetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY);

            List<EntraIdFilterDB> currentFilterAttributes = [];
            if (!string.IsNullOrWhiteSpace(currentFilterAttributesAsJsonString))
            {
                currentFilterAttributes = JsonSerializer.Deserialize<List<EntraIdFilterDB>>(currentFilterAttributesAsJsonString) ?? [];
            }

            EntraIdFilterDB? filterToUpdate = currentFilterAttributes?.Find(filterFind => filterFind.Id == filterId);
            if (filterToUpdate == null)
            {
                throw new EntraIdFilterNotFoundException($"Filter with id '{filterId}'");
            }

            filterToUpdate.FilterAttribute = filter.FilterAttribute;
            filterToUpdate.Condition = filter.Condition;
            filterToUpdate.FilterValue = filter.FilterValue;
            filterToUpdate.FilterString = filter.Condition.Replace("{1}", filter.FilterValue).Replace("{2}", filter.FilterAttribute);

            FilterValidationResponse validation = await ValidateFilter(filterToUpdate);

            filterToUpdate.IsValid = validation.Success;
            filterToUpdate.ValidationMessage = validation.Message;

            await _settingsRepository.SetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY, JsonSerializer.Serialize(currentFilterAttributes));

            await _settingsRepository.UpdateRuntimeInfo();
        }

        public async Task CreateEntraIdFilter(EntraIdFilterPayload filter)
        {
            string filterString = filter.Condition.Replace("{1}", filter.FilterValue).Replace("{2}", filter.FilterAttribute);
            EntraIdFilterDB entraIdFilter = new EntraIdFilterDB(filter.FilterAttribute, filter.Condition, filter.FilterValue, filterString);

            FilterValidationResponse validation = await ValidateFilter(entraIdFilter);
            entraIdFilter.IsValid = validation.Success;
            entraIdFilter.ValidationMessage = validation?.Message;

            string? currentFilterAttributesAsJsonString = await _settingsRepository.GetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY);
            List<EntraIdFilterDB> dbFilterAttributes = [];
            if (!string.IsNullOrWhiteSpace(currentFilterAttributesAsJsonString))
            {
                dbFilterAttributes = JsonSerializer.Deserialize<List<EntraIdFilterDB>>(currentFilterAttributesAsJsonString) ?? [];
            }

            if (dbFilterAttributes.Count >= 5)
            {
                throw new EntraIdFilterLimitExceededException("Maximum of 5 filters allowed");
            }

            dbFilterAttributes?.Add(entraIdFilter);

            await _settingsRepository.SetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY, JsonSerializer.Serialize(dbFilterAttributes));

            await _settingsRepository.UpdateRuntimeInfo();
        }

        public async Task DeleteEntraIdFilter(Guid id)
        {
            string? currentFilterAttributesAsJsonString = await _settingsRepository.GetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY);
            List<EntraIdFilterDB> dbFilterAttributes = [];
            if (!string.IsNullOrWhiteSpace(currentFilterAttributesAsJsonString))
            {
                dbFilterAttributes = JsonSerializer.Deserialize<List<EntraIdFilterDB>>(currentFilterAttributesAsJsonString) ?? [];
            }

            EntraIdFilterDB? filterToRemove = dbFilterAttributes.Find(filter => filter.Id == id);

            if (filterToRemove == null)
            {
                throw new EntraIdFilterNotFoundException($"Filter with id '{id}'");
            }

            dbFilterAttributes.Remove(filterToRemove);
            await _settingsRepository.SetValue(UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_ID, UnifiedContactStaticStrings.SETTINGS_ENTRAIDFILTER_CATEGORY, JsonSerializer.Serialize(dbFilterAttributes));

            await _settingsRepository.UpdateRuntimeInfo();
        }

        private async Task<FilterValidationResponse> ValidateFilter([FromBody] EntraIdFilterDB filterToValidate)
        {
            Microsoft.Graph.GraphServiceClient graphclient = _graphApiEngine.AuthorizeWithApplicationPermissions();
            string filter = filterToValidate.FilterString;
            try
            {
                UserCollectionResponse? response = await graphclient.Users.GetAsync((requestConfiguration) =>
                {
                    requestConfiguration.QueryParameters.Filter = filter;
                    requestConfiguration.QueryParameters.Select = new string[] { "id" };
                    requestConfiguration.Headers.Add("ConsistencyLevel", "eventual");
                    requestConfiguration.QueryParameters.Count = true;
                });
                FilterValidationResponse filterValidationResponse = new FilterValidationResponse(success: true);
                return filterValidationResponse;
            }
            catch (Exception e)
            {
                FilterValidationResponse filterValidationResponse = new FilterValidationResponse(success: false, message: e.Message);
                return filterValidationResponse;
            }
        }
    }
}