namespace ABMVantage.Data.Models
{
    public class SingleTicketEVChargesResponse
    {
        public string? Id {  get; set; }
        public string? SessionId { get; set; }
        public string? TicketId { get; set; }
        public string? Lpn { get; set; }
        public DateTime? SessionStartTimeInUtc { get; set; }
        public DateTime? SessionEndTimeInUtc { get; set; }
        public string? EvChargingStationID { get; set; } //Need to get Charger Type and then do a lookup using Type for the Unit Cost/ServiceFee
        public double TotalKwhUsed { get; set; }
        public double UnitCost { get { return 0.45; } } //Temp UnitCosts [Would come from Master Data for the Site]
        public double EvChargingFee
        {
            get
            {
                return (this.TotalKwhUsed * this.UnitCost);
            }
        }
        public double EvChargingTaxFee { get { return 0; } } //[No Tax Fee]
        public double ServiceChargeFee { get { return 2; } } //Temp Service Charge Fee [Would come from Master Data for the Site]
        public double EvChargingTotalFee
        {
            get
            {
                return EvChargingFee + this.EvChargingTaxFee + ServiceChargeFee;
            }
        }
    }
}
