namespace ABMVantage_Outbound_UnitTests.Utility.TestDataBuilder
{
    using ABMVantage.Data.Models;

    public static class TestDataBuilder
    {
        public static List<RevenueBudget> GetRevenueBudgetTestData()
        {
            return new List<RevenueBudget>()
            {
                new RevenueBudget
                {
                    BudgetedRevenue = 100,
                    Month = "MAR",
                    Revenue = 200
                },
                new RevenueBudget
                {
                    BudgetedRevenue = 300,
                    Month = "APR",
                    Revenue = 499
                },
                new RevenueBudget
                {
                    BudgetedRevenue = 20,
                    Month = "MAY",
                    Revenue = 200
                },
                new RevenueBudget
                {
                    BudgetedRevenue = 10,
                    Month = "JUN",
                    Revenue = 20000
                }
            };
        }
        public static List<BudgetVariance> GetBudgetVarianceTestData()
        {
            return new List<BudgetVariance>()
            {
                new BudgetVariance
                {
                    BgtVariance = 100,
                    Month = "MAR",                    
                },
                new BudgetVariance
                {
                    BgtVariance = 300,
                    Month = "APR",
                },
                new BudgetVariance
                {
                    BgtVariance = 20,
                    Month = "MAY",
                },
                new BudgetVariance
                {
                    BgtVariance = 10,
                    Month = "JUN",
                }
            };
        }
    }
}