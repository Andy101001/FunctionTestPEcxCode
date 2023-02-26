namespace ABMVantage_Outbound_API.EntityModels
{
    using Newtonsoft.Json;
    public class PgsOccupancy : BaseSharedEntityModel
    {
        public PgsOccupancy()
        {
            HighLevelBusinessEntity = "pgsTicketOccupancies";
        }

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("occupancyId")]
        public string? OccupancyId { get; set; }

        [JsonProperty("ticketID")]
        public string? TicketID { get; set; }

        [JsonProperty("facitlityId")]
        public string? FacitlityId { get; set; }

        [JsonProperty("levelId")]
        public string? LevelId { get; set; }

        [JsonProperty("spaceId")]
        public string? SpaceId { get; set; }

        [JsonProperty("bayId")]
        public string? BayId { get; set; }

        [JsonProperty("occupancyEntryDateTime")]
        public string? OccupancyEntryDateTime { get; set; }

        [JsonProperty("occupancyExitDatetime")]
        public string? OccupancyExitDatetime { get; set; }

        [JsonProperty("exitDateTime")]
        public string? ExitDateTime { get; set; }

        [JsonProperty("lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("occupied")]
        public bool? Occupied { get; set; }
    }
}