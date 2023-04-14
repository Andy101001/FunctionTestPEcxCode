namespace ABMVantage_Outbound_API.Models
{
    using Newtonsoft.Json;
    
    /// <summary>
    /// Reservation by hour result
    /// </summary>
    public class ReservationByHour
    {
        [JsonProperty("reservationTime")]
        public string ReservationTime { get; set; }
        
        [JsonProperty("data")]
        public List<ReservationByHourData> Data { get; set; }
    }

    /// <summary>
    /// Reservation by hour data
    /// </summary>
    public class ReservationByHourData
    {
        [JsonProperty("product")]
        public string Product { get; set; }
        [JsonProperty("noOfReservations")]
        public int NoOfReservations { get; set; }
    }
}