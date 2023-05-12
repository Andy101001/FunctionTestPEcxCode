using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.Models
{
    public class StgFilterData
    {
        public string? CustomerId { get; set; }
        public string BuCode { get; set; }
        public string? FacilityId { get; set; }
        public string? FacilityName { get; set; }
        public string? LevelId { get; set; }
        public string? Level { get; set; }
        public int ProductId { get; set; }
        public string? ProductName { get; set; }

    }
}
