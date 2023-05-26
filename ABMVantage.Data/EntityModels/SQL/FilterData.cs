using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABMVantage.Data.EntityModels.SQL
{
    [Table("FilterData")]
    public class FilterDataSQL
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [Column("CustomerId")]
        public int CustomerId { get; set; }

        [Column("BuCode")]
        public string? BuCode { get; set; }

        [Column("FacilityId")]
        public string? FacilityId { get; set; }

        [Column("FacilityName")]
        public string? FacilityName { get; set; }

        [Column("LevelId")]
        public string? LevelId { get; set; }

        [Column("Level")]
        public string? Level { get; set; }

        [Column("ProductId")]
        public int ProductId { get; set; }

        [Column("ProductName")]
        public string? ProductName { get; set; }
    }
}
