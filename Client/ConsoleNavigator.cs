using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using Shared;

namespace Client
{
    public static class ConsoleNavigator
    {
        public static void SignUp(out User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Please enter your first name");
            var firstName = Console.ReadLine();
            Console.WriteLine("Please enter your last name");
            var lastName = Console.ReadLine();
            Console.WriteLine("Please enter your email");
            var email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            var password = Console.ReadLine();

            using (var client = new HttpClient())
            {
                var newUser = new User(firstName, lastName, email, password) { };
                var newUserJson = JsonConvert.SerializeObject(newUser);
                var endpoint = new Uri("https://stockpricing.azurewebsites.net/values/createUser");
                var payload = new StringContent(newUserJson, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<User>(response);
                currentUser = user;

                Console.Clear();
                Console.WriteLine("Profile created!");
                Thread.Sleep(1000);
            }
        }
        public static void SignIn(out User currentUser)
        {
            Console.Clear();
            Console.WriteLine("Please enter your email");
            var email = Console.ReadLine();
            Console.WriteLine("Please enter your password");
            var password = Console.ReadLine();

            using (var client = new HttpClient())
            {
                var endpoint = new Uri($"https://stockpricing.azurewebsites.net/values/getUser?email={email}&password={password}");

                var response = client.GetAsync(endpoint).Result; //Content.ReadAsStringAsync().Result;
                var responeBody = response.Content.ReadAsStringAsync().Result;
                var user = JsonConvert.DeserializeObject<User>(responeBody);
                currentUser = user;
                if (user == null)
                {
                    Console.WriteLine("Wrong email or password. Please try again.");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine($"Welcome back {user.FirstName}!");
                    Thread.Sleep(1000);
                }
            }
        }
        public static void GetAllStocksWithPrice()
        {
            //                      <<< Price stream simulator >>>
            ConsoleKeyInfo cki;
            do
            {
                while (Console.KeyAvailable == false)
                    using (var client = new HttpClient())
                    {
                        var endpoint = new Uri("https://stockpricing.azurewebsites.net/values/getAllStocksPricing");
                        var response = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
                        dynamic stocksJson = JsonConvert.DeserializeObject(response.ToString());

                        Console.Clear();
                        Console.WriteLine("Press 'x' to stop pricing");
                        Console.WriteLine();

                        foreach (var stock in stocksJson)
                        {
                            Console.WriteLine(stock);
                        }               
                    }
                cki = Console.ReadKey(true);
            } while (cki.Key != ConsoleKey.X);
        }
        public static void Subscribe(User currentUser)
        {
            Dictionary<int, List<string>> userStocks = new Dictionary<int, List<string>>() { };
            userStocks[currentUser.UserId] = new List<string>();

            var _continue = "yes";
            while (_continue == "yes")
            {
                Console.Clear();
                Console.WriteLine("Choose the stock");
                Console.WriteLine("[1] - MSFT");
                Console.WriteLine("[2] - AAPL");
                Console.WriteLine("[3] - AMZN");
                var option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    // TODO: Add a check if user already subscribed
                    case 1:
                        userStocks[currentUser.UserId].Add("MSFT");
                        break;
                    case 2:
                        userStocks[currentUser.UserId].Add("AAPL");
                        break;
                    case 3:
                        userStocks[currentUser.UserId].Add("AMZN");
                        break;
                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
                Console.WriteLine("Would you like to add more? [yes/no]");
                _continue = Console.ReadLine();
            }
            using (var client = new HttpClient())
            {
                var subscriptionsJson = JsonConvert.SerializeObject(userStocks);
                var endpoint = new Uri("https://stockpricing.azurewebsites.net/values/subscribe");
                var payload = new StringContent(subscriptionsJson, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                Console.Clear();
                Console.WriteLine("You subscribed to:");
                foreach (var stock in userStocks[currentUser.UserId])
                {
                    Console.WriteLine($"{stock}");
                }
                Thread.Sleep(1500);
                Console.Clear();
            }
        }
        public static void UnSubscribe(User currentUser)
        {
            Dictionary<int, List<string>> userStocks = new Dictionary<int, List<string>>() { };
            userStocks[currentUser.UserId] = new List<string>();

            var _continue = "yes";
            while (_continue == "yes")
            {
                Console.Clear();
                Console.WriteLine("Choose the stock to unsubscribe from:");
                Console.WriteLine("[1] - MSFT");
                Console.WriteLine("[2] - AAPL");
                Console.WriteLine("[3] - AMZN");
                var option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    // TODO: Add a check if user already subscribed
                    case 1:
                        userStocks[currentUser.UserId].Add("MSFT");
                        break;
                    case 2:
                        userStocks[currentUser.UserId].Add("AAPL");
                        break;
                    case 3:
                        userStocks[currentUser.UserId].Add("AMZN");
                        break;
                    default:
                        Console.WriteLine("Wrong input");
                        break;
                }
                Console.WriteLine("Would you like to add more? [yes/no]");
                _continue = Console.ReadLine();
            }
            using (var client = new HttpClient())
            {
                var subscriptionsJson = JsonConvert.SerializeObject(userStocks);
                var endpoint = new Uri("https://stockpricing.azurewebsites.net/values/unsubscribe");
                var payload = new StringContent(subscriptionsJson, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, payload).Result.Content.ReadAsStringAsync().Result;

                Console.Clear();
                Console.WriteLine("Unsubscribed successfully!");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }
        public static void GetUserStocks(User currentUser)
        {
            Console.Clear();
            using (var client = new HttpClient())
            {
                var endpoint = new Uri($"https://stockpricing.azurewebsites.net/values/getAllUserStocks?UserId={currentUser.UserId}");
                var response = client.GetAsync(endpoint).Result.Content.ReadAsStringAsync().Result;
                var stocks = JsonConvert.DeserializeObject<List<string>>(response);
                if (stocks.Count == 0)
                {
                    Console.WriteLine("No subscribed stocks");
                }
                else
                {
                    foreach (var stock in stocks)
                    {
                        Console.WriteLine($"{stock}");
                    }
                }
            }
            Console.WriteLine();
        }
    }
}
