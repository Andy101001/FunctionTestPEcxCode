using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class MSCharagerInitiateRequest
    {
        public string QrCode { get; set; }
        public string TicketOrReservationId { get; set; }
        public string lpn { get; set; }
    }
}
