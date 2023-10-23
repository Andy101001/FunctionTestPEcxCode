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
        public bool IsDataForOneDate { get; set; } = false;
        public string displayTimeFrameText
        {
            get
            {
                if(!IsDataForOneDate)
                return $"Data from {this.FromDate.ToShortDateString()} to {this.ToDate.ToShortDateString()}";
                else
                   return $"Data on {this.FromDate.ToShortDateString()}";
            }
        }

    }
}
