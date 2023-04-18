using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    public class MonthlyAverageTicketValue
    {
        public int Year { get; internal set; }
        public int Month { get; internal set; }
        public decimal AverageTicketValue { get; set; }
        public string ParkingProduct { get; set; }
    }
}
