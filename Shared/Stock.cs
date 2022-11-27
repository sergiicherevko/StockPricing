using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class Stock
    {
        public int StockId { get; set; }

        public string StockName { get; set; }

        public string StockSymbol { get; set; }

        public double PriceRangeLow { get; set; }

        public double PricRangeHigh { get; set; }
    }
}
