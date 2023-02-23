namespace ABMVantage_Outbound_API.EntityModels
{
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;


    public class EvActiveSessions : BaseSharedEntityModel
    {
        public EvActiveSessions()
        {
            HighLevelBusinessEntity = "evChargingSessionsActive";
        }

        [Required]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("responseActiveChargeSessions")]
        public List<ActiveChargeSession>? ResponseActiveChargeSession { get; set; }
    }
    public class ActiveChargeSession
    {
        /// <summary>
        /// Defined for EF indexing on fk
        /// </summary>
        public EvActiveSessions? Parent { get; set; }

        [JsonProperty("pfid")]
        public string Id { get; set; }

        [JsonProperty("kwHoursDelivered")]
        public int? KWHoursDelivered { get; set; }
        
        [JsonProperty("chargeSessionId")]
        public string? ChargeSessionId { get; set; }

        [JsonProperty("parkingSpace")]
        public string? ParkingSpace { get; set; }

        [JsonProperty("chargerId")]
        public string? ChargerId { get; set; }

        [JsonProperty("chargeStartDateTime")]
        public DateTime? ChargeStartDateTime { get; set; }
        
        [JsonProperty("ticketId")]
        public string ?TicketId { get; set; }

        [JsonProperty("lpn")]
        public string ?Lpn { get; set; }

        [JsonProperty("reservationId")]
        public string ?ReservationId { get; set; }
    }
}