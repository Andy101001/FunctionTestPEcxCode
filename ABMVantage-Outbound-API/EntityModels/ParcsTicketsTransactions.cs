namespace ABMVantage_Outbound_API.EntityModels
{
    using Newtonsoft.Json;
    public class ParcsTicketsTransactions : BaseSharedEntityModel
    {

        public ParcsTicketsTransactions()
        {
            HighLevelBusinessEntity = "parcsTicketsTransactions";
        }

        [JsonProperty("chargerId")]
        public string? ChargerId { get; set; }

        [JsonProperty("ticketID")]
        public string? TicketID { get; set; }

        [JsonProperty("chargingSessionID")]
        public string? ChargingSessionID { get; set; }

        [JsonProperty("reservationId")]
        public string? ReservationId { get; set; }

        [JsonProperty("chargeStartDateTime")]
        public DateTime? ChargeStartDateTime { get; set; }

        [JsonProperty("chargeEndDateTime")]
        public DateTime? ChargeEndDateTime { get; set; }

        [JsonProperty("lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("kWHoursDelivered")]
        public string? KWHoursDelivered { get; set; }

        [JsonProperty("amountCharged")]
        public double? AmountCharged { get; set; }

        [JsonProperty("fee")]
        public double? Fee { get; set; }

    }
}