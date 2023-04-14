using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("FACT_PAYMENTS_TICKETS_AND_STAGED")]
    public class FactPaymentsTicketAndStaged
    {
        [Column("PAYMENT_ID")]
        public string? PaymentId { get; set; }

        [Column("PAYMENT_DATETIME_UTC")]
        public string? PaymentDateTimeUtc { get; set; }

        [Column("AMOUNT")]
        public  decimal Amount { get; set; }

        [Column("Ticket_Id")] 
        public string? TicketId { get; set; }

    }
}
