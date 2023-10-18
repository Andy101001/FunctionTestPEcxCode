using ABMVantage.Data.Models;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ABMVantage.Data.EntityModels
{
    public class SingleTicketEV
    {
        [Key]
        [JsonProperty("SingleTicketEvId")]
        public string? SingleTicketEvId { get; set; }

        [JsonProperty("ChargeSessionId")]
        public string? ChargeSessionId { get; set; }

        [JsonProperty("ClientId")]
        public string? ClientId { get; set; }

        [JsonProperty("Lpn")]
        public string? Lpn { get; set; }

        [JsonProperty("ParkingSpaceId")]
        public string? ParkingSpaceId { get; set; }

        [JsonProperty("RawTicketId")]
        public string? RawTicketId { get; set; }

        [JsonProperty("TicketId")]
        public string? TicketId { get; set; }

        [JsonProperty("EvChargeSessions")]
        public List<EvChargeSession> EvChargeSessions { get; set; } = new ();

    }


    public class EvChargeSession
    {
        [JsonProperty("KWHoursDelivered")]
        public double KWHoursDelivered { get; set; }

        [JsonProperty("ChargeStartDateTimeUTC")]
        public DateTime? ChargeStartDateTimeUTC { get; set; }

        [JsonProperty("ChargeEndDateTimeUTC")]
        public DateTime? ChargeEndDateTimeUTC { get; set; }

        [JsonProperty("ParkingSpace")]
        public string? ParkingSpace { get; set; }

        [JsonProperty("Fee")]
        public double? Fee { get; set; }

    }

}
