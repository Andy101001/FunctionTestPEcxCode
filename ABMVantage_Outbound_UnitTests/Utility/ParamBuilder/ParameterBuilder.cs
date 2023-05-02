namespace ABMVantage_Outbound_UnitTests.Utility.ParamBuilder
{
    using ABMVantage.Data.Models;

    public static class ParameterBuilder
    {
        public static FilterParam GetFilterParams()
        {
            return new FilterParam
            {
                CustomerId = 1,
                Facilities = new List<FacilityFilter>
                {
                    new FacilityFilter
                    {
                        Id = "LAX3576BLDG01",
                        Name = "LAXPARKINGBLDG01"
                    }
                },
                ParkingLevels = new List<LevelFilter>
                {
                    new LevelFilter
                    {
                        Id = "AGPK01_05",
                        Level = 5,
                    }
                },
                UserId = "UT",
                FromDate = DateTime.Now.Subtract(TimeSpan.FromDays(15)),
                ToDate = DateTime.Now.AddMonths(3)
            };
        }
    }
}