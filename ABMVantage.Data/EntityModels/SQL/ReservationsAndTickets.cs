using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("RptReservationsByHour")]
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

    [Table("RptReservationsSpanningHour")]
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


    [Table("RptReservationsSpanningDay")]
    public class ReservationSpanningDaySQL
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

        [Column("BeginningOfDay")]
        public DateTime BeginningOfDay { get; set; }

        [Column("NoOfReservations")]
        public int NoOfReservations { get; set; }

    }



    [Table("RptReservationsSpanningMonth")]
    public class ReservationSpanningMonthSQL
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

        [Column("BeginningOfMonth")]
        public DateTime BeginningOfMonth { get; set; }

        [Column("NoOfReservations")]
        public int NoOfReservations { get; set; }

    }
}
