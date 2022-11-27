using Microsoft.EntityFrameworkCore;
using Shared;

namespace Server.Models
{
    public class StockPricingContext : DbContext
    {
        public StockPricingContext(DbContextOptions<StockPricingContext> options) : base(options) { }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder.Entity<Stock>().HasData(
                new Stock
                {
                    StockId = 1,
                    StockName = "MicroSoft",
                    StockSymbol = "MSFT",
                    PriceRangeLow = 250.00,
                    PricRangeHigh = 290.00
                },
                new Stock
                {
                    StockId = 2,
                    StockName = "Apple",
                    StockSymbol = "AAPL",
                    PriceRangeLow = 150.00,
                    PricRangeHigh = 165.00
                },
                new Stock
                {
                    StockId = 3,
                    StockName = "Amazon",
                    StockSymbol = "AMZN",
                    PriceRangeLow = 120.00,
                    PricRangeHigh = 130.00
                });            
        }
    }
}
