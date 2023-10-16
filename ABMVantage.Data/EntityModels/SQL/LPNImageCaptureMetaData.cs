using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("LPNImageCaptureMetaData")]
    public class LPNImageCaptureMetaData
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("EventName")]
        public string? EventName { get; set; }

        [Column("UID")]
        public string? UID { get; set; }

        [Column("AreaType")]
        public int? AreaType { get; set; }

        [Column("Mode")]
        public int Mode { get; set; }

        [Column("EnterUTC")]
        public DateTime? EnterUTC { get; set; }

        [Column("ExitUTC")]
        public DateTime? ExitUTC { get; set; }

        [Column("TimeStampUTC")]
        public DateTime? TimeStampUTC { get; set; }

        [Column("LPRTimeStampUTC")]
        public DateTime? LPRTimeStampUTC { get; set; }

        [Column("LPR")]
        public string? LPR { get; set; }

        [Column("LPRCode")]
        public int? LPRCode { get; set; }

        [Column("LPN")]
        public string? LPN { get; set; }

        [Column("Occupied")]
        public bool? Occupied { get; set; }


        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }


        [Column("CreatedBy")]
        public string? CreatedBy { get; set; }

        
    }
}
