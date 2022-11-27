using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using Shared;

namespace Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private StockPricingContext context { get; set; }
        public ValuesController(StockPricingContext stockPricingContext)
        {
            context = stockPricingContext;
        }

        [HttpGet("getAllStocksPricing")]
        public List<string> getAllStocksPricing()
        {
            Thread.Sleep(1000);
            List<string> stocks = new List<string>();
            foreach (var stock in context.Stocks)
            {
                var price = Math.Round(PriceGeneratorService.GeneratePrice(stock), 2);
                var symbol = stock.StockSymbol;
                stocks.Add($"[{symbol}]  :  [{price}]");
            }            
            return stocks;
        }

        [HttpGet("getAllUserStocks")]
        public List<string> getAllUserStocks(int UserId) 
        {
            var stocks = (from u in context.Users
                                   join us in context.UserStocks
                                   on u.UserId equals us.UserId
                                   join s in context.Stocks
                                   on us.StockId equals s.StockId
                                   select new { s.StockSymbol, u.UserId })
                            .Where(o => o.UserId == UserId)
                            .Select(s => s.StockSymbol)
                            .ToList();
            return stocks;
        }

        [HttpPost("createUser")]
        public User createUser([FromBody] User userInfo)
        {
            var user = new User(userInfo.FirstName, userInfo.LastName, userInfo.Email, userInfo.Password);
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        [HttpGet("getUser")]
        public User getUser(string email, string password)
        {
            try
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                return user;
            }
            catch (Exception)
            {
                Console.WriteLine("Wrong email or password");
                return null;
            }            
        }

        [HttpPost("subscribe")]
        public void subscribe([FromBody] Dictionary<int, List<string>> userNewStocks)
        {
            foreach (var userNewStock in userNewStocks)
            {
                var userId = userNewStock.Key;
                var symbols = userNewStock.Value;
                var user = context.Users.FirstOrDefault(u => u.UserId == userId);
                foreach (var symbol in symbols)
                {
                    var stock = context.Stocks.FirstOrDefault(s => s.StockSymbol == symbol);
                    var userStock = new UserStock(userId, stock.StockId);
                    context.UserStocks.Add(userStock);
                }
            }
            context.SaveChanges();
        }

        [HttpPost("unsubscribe")]
        public void unsubscribe([FromBody] Dictionary<int, List<string>> userNewStocks) 
        {
            foreach (var userNewStock in userNewStocks)
            {
                var userId = userNewStock.Key;
                var symbols = userNewStock.Value;
                foreach (var symbol in symbols)
                {
                    var stock = context.Stocks.FirstOrDefault(s => s.StockSymbol == symbol);
                    var userStock = context.UserStocks
                        .FirstOrDefault(us => us.UserId == userId && us.StockId == stock.StockId);
                    if (userStock != null){context.UserStocks.Remove(userStock);}
                }
            }
            context.SaveChanges();
        }
    }
}
