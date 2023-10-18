namespace ABMVantage.Data.Models
{
    public class SingleTicketEVChargesResponse
    {
        public string? SingleTicketEvId { get; set; }
        public string? ChargeSessionId { get; set; }
        public string? ClientId { get; set; }
        public string? TicketId { get; set; }
        public string? RawTicketId { get; set; }
        public string? Lpn { get; set; }

        public DateTime? SessionStartTimeInUtc { get; set; }
        public DateTime? SessionEndTimeInUtc { get; set; }
        public string? ParkingSpaceId { get; set; } //Need to get Charger Type and then do a lookup using Type for the Unit Cost/ServiceFee
        public double TotalKWHoursDelivered { get; set; }
        public double? UnitCost { get { return EvChargingFee / TotalKWHoursDelivered; } } //Temp UnitCosts [Would come from Master Data for the Site]
        public double? EvChargingFee { get; set; }
        public double EvChargingTaxFee { get; set; }
        public double ServiceChargeFee { get; set; }
        public double? EvChargingTotalFee
        {
            get
            {
                return EvChargingFee + this.EvChargingTaxFee + ServiceChargeFee;
            }
        }
    }
}
