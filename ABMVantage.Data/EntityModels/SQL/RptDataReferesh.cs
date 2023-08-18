using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("RptDataReferesh")]
    public class RptDataReferesh
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("TableName")]
        public string TableName { get; set; }

        [Column("ChartName")]
        public string ChartName { get; set; }

        [Column("ChartKeyName")]
        public string ChartKeyName { get; set; }

        [Column("PageName")]
        public string PageName { get; set; }

        [Column("DataRefreshDate")]
        public DateTime DataRefreshDate { get; set; }

        
    }
}
