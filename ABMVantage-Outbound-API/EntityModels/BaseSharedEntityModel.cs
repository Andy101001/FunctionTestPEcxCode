namespace ABMVantage_Outbound_API.EntityModels
{
    using System.Text.Json.Serialization;
    public class BaseSharedEntityModel
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("highLevelBusinessEntity")]
        public string HighLevelBusinessEntity { get; set; }
    }
}