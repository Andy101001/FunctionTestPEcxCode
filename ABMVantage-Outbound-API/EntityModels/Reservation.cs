namespace ABMVantage_Outbound_API.EntityModels
{ 
    /// <summary>
    /// TEMPORARY MOCK DATA FOR UI
    /// </summary>
    public class Reservation
    {
        public string Id { get; set; }
        public List<string> ReservationsLabels { get; set; } = new List<string>();
        public List<int> ReservationsEvData { get; set; } = new List<int>();
        public List<int> ReservationsValetData { get; set; } = new List<int>();
        public List<int> ReservationsPremiumData { get; set; } = new List<int>();
        public List<int> ReservationsGeneralData { get; set; } = new List<int>();
        public List<int> ReservationsMonthlyData { get; set; } = new List<int>();
        public string ReservationsChartData { get; set; }
        public string ReservationsChartOptions { get; set; }
    }
}