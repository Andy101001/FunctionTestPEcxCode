using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class DataReferesh
    {
        public string ChartName { get; set; }
        public string PageName { get; set; }
        public string ChartKeyName { get; set; }
        public DateTime DataRefreshDate { get; set; }
    }
}
