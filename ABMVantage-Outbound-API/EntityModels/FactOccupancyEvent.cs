using Fare;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.EntityModels
{
    [Table("FACT_OCCUPANCY_EVENT]")]
    public class FactOccupancyEvent
    {
        [Column("OCCUPANCY_ID")]
        public string? OccupancyId { get;set; }

        [Column("Ticket_Id")]
        public string? TicketId { get; set; }

        [Column("Facitly_Id")]
        public string? FacilityId { get; set; }

        [Column("Level_ID")]
        public string? LevelId { get; set; }

        [Column("CCUPANCY_ENTRY_DATETIME_UTC")]
        public DateTime? OccupancyEventDateTimeUtc { get; set; }

        // 
    }
}
