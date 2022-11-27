using System;
using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Shared;

namespace Server.Services
{
    public static class PriceGeneratorService
    {    
        public static double GeneratePrice(Stock stock) 
        {
            Random random = new Random();
            return random.NextDouble() * (stock.PricRangeHigh - stock.PriceRangeLow) + stock.PriceRangeLow;
        }
    }
}
