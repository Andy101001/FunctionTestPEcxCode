namespace ABMVantage_Outbound_API.EntityModels
{
    using Newtonsoft.Json;
    public class ObsReservationTransactions: BaseSharedEntityModel
    {
        [JsonProperty("id")]
        public string? id { get; set; }

        [JsonProperty("reservationId")]
        public string? ReservationId { get; set; }

        [JsonProperty("locationId")]
        public string? LocationId { get; set; }

        [JsonProperty("ticketID")]
        public string? TicketID { get; set; }

        [JsonProperty("reservationBarcode")]
        public string? ReservationBarcode { get; set; }

        [JsonProperty("reservedEntryDateTime")]
        public DateTime? ReservedEntryDateTime { get; set; }

        [JsonProperty("reservedExitDatetime")]
        public DateTime? ReservedExitDatetime { get; set; }

        [JsonProperty("exitDateTime")]
        public DateTime? ExitDateTime { get; set; }

        [JsonProperty("lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("spaceReserved")]
        public bool? SpaceReserved { get; set; }

    }
}