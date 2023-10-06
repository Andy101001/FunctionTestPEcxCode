using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class MSPageLoadRequest
    {
        public string QrCode { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
