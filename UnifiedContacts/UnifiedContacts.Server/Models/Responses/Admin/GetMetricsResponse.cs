using System.Text.Json.Serialization;

namespace UnifiedContacts.Models.Responses.Admin
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$metricType")]
    [JsonDerivedType(typeof(QuotaMetric), typeDiscriminator: "quota")]
    [JsonDerivedType(typeof(DisplayNumberMetric), typeDiscriminator: "displayNumber")]
    public abstract class Metric
    {
        public string DisplayName { get; set; }

        [JsonConstructor]
        public Metric(string displayName)
        {
            DisplayName = displayName;
        }
    }

    public class QuotaMetric : Metric
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonConstructor]
        public QuotaMetric(string displayName, int value, int? limit = null) : base(displayName)
        {
            Limit = limit;
            Value = value;
        }
    }

    public class DisplayNumberMetric : Metric
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
        [JsonPropertyName("limit")]
        public int? Limit { get; set; }

        [JsonConstructor]
        public DisplayNumberMetric(string displayName, int value, int? limit = null) : base(displayName)
        {
            Limit = limit;
            Value = value;
        }
    }

    public class GetMetricsResponse
    {
        public IEnumerable<Metric> Metrics { get; set; } = Enumerable.Empty<Metric>();
    }
}
