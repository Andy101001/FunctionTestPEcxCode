using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("FACT_TICKETS]")]
    public class FactTicket
    {
        [Column("TICKET_ID")]
        public string? TicketId { get; set; }

        [Column("FACILITY_ID")]
        public string? FacilityId { get; set; }
    }
}
