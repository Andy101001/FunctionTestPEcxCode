using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("RevenueAndBudget")]
    public class RevenueAndBudgetSQL
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
        [Column("ProductName")]
        public string ProductName { get; set; }
        [Column("FirstDayOfMonth")]
        public DateTime FirstDayOfMonth { get; set; }
        [Column("Revenue")]
        public decimal Revenue { get; set; }
        [Column("BudgetedRevenue")]
        public decimal BudgetedRevenue { get; set; }

    }
}
