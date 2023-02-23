namespace ABMVantage_Outbound_API.EntityModels
{
    using Newtonsoft.Json;

    public class EvClosedSessions : BaseSharedEntityModel
    {
        public EvClosedSessions() 
        {
            HighLevelBusinessEntity = "evChargingSessionsClosed";
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("chargerID")]
        public string? ChargerId { get; set; }

        [JsonProperty("chargingSessionID")]
        public string? ChargeSessionId { get; set; }

        [JsonProperty("kwHoursDelivered")]
        public int? KWHoursDelivered { get; set; }

        [JsonProperty("chargeStartDateTime")]
        public DateTime? ChargeStartDateTime { get; set; }

        [JsonProperty("chargeEndDateTime")]
        public DateTime? ChargeEndDateTime { get; set; }
        
        [JsonProperty("ticketId")]
        public string? TicketId { get; set; }

        [JsonProperty("lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("reservationId")]
        public string? ReservationId { get; set; }
    }
}