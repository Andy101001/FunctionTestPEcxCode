using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{

    [Table("RevenueTransaction")]
    public class RevenueTransactionSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

       [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }

        [Column("ProductName")]
        public string ProductName { get; set; }
    }


    [Table("RevenueRevenueVsBudget")]
    public class RevenueRevenueVsBudgetSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [Column("Revenue")]
        public decimal Revenue { get; set; }

        [Column("BudgetedRevenue")]
        public decimal BudgetedRevenue { get; set; }

        [Column("Year")]
        public int Year { get; set; }

        [Column("Month")]
        public int Month { get; set; }
    }


    [Table("Revenuebyday")]
    public class RevenuebydaySQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("Product")]
        public string? Product { get; set; }

        [Column("TransactionId")]
        public DateTime TransactionId { get; set; }

        [Column("Revenue")]
        public decimal Revenue { get; set; }

        [Column("BudgetedRevenue")]
        public decimal BudgetedRevenue { get; set; }
    }

    [Table("RevenueBudgetVsActualVariance")]
    public class RevenueBudgetVsActualVarianceSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("Month")]
        public string? Month { get; set; }

        [Column("Bgtvariance")]
        public decimal Bgtvariance { get; set; }

        [Column("MonthId")]
        public int MonthId { get; set; }
    }

}
