using System.Text.Json.Serialization;

namespace UnifiedContacts.Models.Payloads
{
    public class SetAllSettingValuesOfCategorySetting
    {
        [JsonPropertyName("settingId")]
        public string SettingId { get; set; }
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        [JsonConstructor]
        public SetAllSettingValuesOfCategorySetting(string settingId, string? value = null)
        {
            SettingId = settingId;
            Value = value;
        }
    }
    public class SetAllSettingValuesOfCategoryPayload
    {
        [JsonPropertyName("values")]
        public IEnumerable<SetAllSettingValuesOfCategorySetting> Values { get; set; } = Enumerable.Empty<SetAllSettingValuesOfCategorySetting>();

    }
}
