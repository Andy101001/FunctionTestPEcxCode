using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("Device")]
    public class DeviceSQL
    {
        [Key]
        [Column("Id")]
        public string Id { get; set; }

        [Column("IsValidDevice")]
        public bool IsValidDevice { get; set; }

        [Column("LocationName")]
        public string? LocationName { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("State")]
        public string State { get; set; }

        [Column("ZipCode")]
        public string ZipCode { get; set; }

        [Column("IsWithAllowedDistance")]
        public bool IsWithAllowedDistance { get; set; }
    }
}
