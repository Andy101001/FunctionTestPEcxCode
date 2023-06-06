using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("Reservation")]
    public class ReservationSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("BeginningOfHour")]
        public DateTime BeginningOfHour { get; set; }

        [Column("NoOfReservations")]
        public int NoOfReservations { get; set; }
        [Column("TotalTicketValue")]
        public decimal TotalTicketValue { get; set; }
    }

    [Table("ReservationsSpanningHour")]
    public class ReservationSpanningHourSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }

        [Column("BeginningOfHour")]
        public DateTime BeginningOfHour { get; set; }

        [Column("NoOfReservations")]
        public int NoOfReservations { get; set; }

    }

    /*
    [Table("ReservationAvgTicket")]
    public class ReservationAvgTicketSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ReservedEntryDateTimeUtc")]
        public DateTime ReservedEntryDateTimeUtc { get; set; }

        [Column("Total")]
        public decimal Total { get; set; }
    }
    */
}
