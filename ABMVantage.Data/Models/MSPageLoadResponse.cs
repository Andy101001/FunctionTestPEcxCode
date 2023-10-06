using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class MSPageLoadResponse
    {
        public string DeviceId { get; set; }
        public bool IsValidDevice { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public bool IsWithInAllowedDistance { get; set; }
    }
}
