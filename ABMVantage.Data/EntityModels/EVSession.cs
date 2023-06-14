using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ABMVantage.Data.EntityModels
{
    public class EVSession
    {
        [Key]
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("ChargeSessionId")]
        public string? ChargeSessionId { get; set; }

        [JsonProperty("ChargeStartDateTimeUTC")]
        public DateTime? ChargeStartDateTimeUTC { get; set; }

        [JsonProperty("ChargeEndDateTimeUTC")]
        public DateTime? ChargeEndDateTimeUTC { get; set; }

        [JsonProperty("TimeStampUTC")]
        public DateTime? TimeStampUTC { get; set; }

        [JsonProperty("ChargerId")]
        public string? ChargerId { get; set; }

        [JsonProperty("ClientId")]
        public string? ClientId { get; set; }

        [JsonProperty("KWHoursDelivered")]
        public double KWHoursDelivered { get; set; }

        [JsonProperty("Lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("ParkingSpace")]
        public string? ParkingSpace { get; set; }

        [JsonProperty("ReservationId")]
        public string? ReservationId { get; set; }

        [JsonProperty("TicketId")]
        public string? TicketId { get; set; }

    }
}
