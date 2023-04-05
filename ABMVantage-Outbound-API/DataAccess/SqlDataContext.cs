using ABMVantage_Outbound_API.EntityModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABMVantage_Outbound_API.DataAccess
{
    public class SqlDataContext: DbContext
    {
        public SqlDataContext(DbContextOptions<SqlDataContext> options) : base(options)
        {
        }
        #region TODO
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Level>();
            //modelBuilder.Entity<Product>();
            //modelBuilder.Entity<Facility>();
            modelBuilder.HasDefaultSchema("BASE");

        }
        #endregion
        /// <summary>
        ///  Get the Levels 
        /// </summary>
        public DbSet<Level> Levels { get; set; }
        /// <summary>
        /// Get Products
        /// </summary>
        public DbSet<Product> Products { get; set; }
        /// <summary>
        /// Get Facilities
        /// </summary>
        public DbSet<Facility> Facilities { get; set; }
    }
}
