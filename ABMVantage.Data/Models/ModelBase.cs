using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class ModelBase
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }

        public string displayTimeFrameText
        {
            get
            {
                return $"Data from {this.FromDate.ToShortDateString()} to {this.ToDate.ToShortDateString()}";
            }
        }

    }
}
