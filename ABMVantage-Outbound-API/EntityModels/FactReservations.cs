using System.ComponentModel.DataAnnotations.Schema;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("FACT_RESERVATIONS")]
    public class FactReservations
    {
        [Column("RESERVATION_ID")]
        public string? ReservationId { get; set; }

        [Column("PRODUCTID")]
        public int? ProductId { get; set; }

        [Column("TICKET_ID")]
        public string? TicketId { get; set; }

        [Column("SOURCE_SYSTEM_ID")]
        public string? SourceSystemId { get; set; }

        [Column("RESERVED_ENTRY_DATETIME_UTC")]
        public DateTime? ReservedEntryDateTimeUTC { get; set; }

        [Column("RESERVED_EXIT_DATETIME_UTC")]
        public DateTime? ReservedExitDateTimeUTC { get; set; }
    }
}