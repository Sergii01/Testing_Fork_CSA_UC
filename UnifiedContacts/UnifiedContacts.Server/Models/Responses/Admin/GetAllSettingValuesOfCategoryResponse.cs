using System.Text.Json.Serialization;

namespace UnifiedContacts.Models.Responses.Admin
{
    /// <summary>
    /// Represents the values of a setting in a category.
    /// </summary>
    public class GetAllSettingValuesOfCategoryValues
    {
        /// <summary>
        /// ID of the setting.
        /// </summary>
        [JsonPropertyName("settingId")]
        public string SettingId { get; set; }

        /// <summary>
        /// The value of the setting.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSettingValuesOfCategoryValues"/> class.
        /// </summary>
        /// <param name="settingId">The ID of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        [JsonConstructor]
        public GetAllSettingValuesOfCategoryValues(string settingId, string? value = null)
        {
            SettingId = settingId;
            Value = value;
        }
    }

    /// <summary>
    /// Represents the response containing all setting values of a category.
    /// </summary>
    public class GetAllSettingValuesOfCategoryResponse
    {
        /// <summary>
        /// ID of the category.
        /// </summary>
        [JsonPropertyName("categoryId")]
        public string CategoryId { get; set; }

        /// <summary>
        /// List of setting values.
        /// </summary>
        [JsonPropertyName("settings")]
        public IEnumerable<GetAllSettingValuesOfCategoryValues> Settings { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSettingValuesOfCategoryResponse"/> class.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <param name="settings">The list of setting values.</param>
        [JsonConstructor]
        public GetAllSettingValuesOfCategoryResponse(string categoryId, IEnumerable<GetAllSettingValuesOfCategoryValues> settings)
        {
            CategoryId = categoryId;
            Settings = settings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSettingValuesOfCategoryResponse"/> class with an empty list of setting values.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        public GetAllSettingValuesOfCategoryResponse(string categoryId)
        {
            CategoryId = categoryId;
            Settings = Enumerable.Empty<GetAllSettingValuesOfCategoryValues>();
        }
    }
}
