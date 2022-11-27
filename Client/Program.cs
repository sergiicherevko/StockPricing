using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Shared;
using System.Threading;

namespace Client
{
    internal class Program
    {
        // TODO:
        // 1. Add Data Validation
        // 2. Add Jwt Auth
        // 3. Add SignalR
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!");
            Console.WriteLine("[1] Sign Up");
            Console.WriteLine("[2] Sign In");

            var option = Int32.Parse(Console.ReadLine());
            User currentUser = null;
            while (currentUser == null)
            {
                switch (option)
                {
                    case 1:
                        ConsoleNavigator.SignUp(out currentUser);
                        break;
                    case 2:
                        ConsoleNavigator.SignIn(out currentUser);
                        if (currentUser == null)
                        {
                            Thread.Sleep(1500);
                            Console.WriteLine("Wrong credentials. Please try again");
                        }
                        break;
                }
            }
            string _continue = "yes";
            while (_continue == "yes")
            {
                Console.Clear();
                Console.WriteLine("What would you like to do?");
                Console.WriteLine("[1] Show available stocks with the current price");
                Console.WriteLine("[2] Show my subscriptions");
                Console.WriteLine("[3] Subscribe");
                Console.WriteLine("[4] Unsubscribe");
                option = Int32.Parse(Console.ReadLine());
                switch (option)
                {
                    case 1:
                        ConsoleNavigator.GetAllStocksWithPrice();
                        break;
                    case 2:
                        ConsoleNavigator.GetUserStocks(currentUser);
                        break;
                    case 3:
                        ConsoleNavigator.Subscribe(currentUser);
                        break;
                    case 4:
                        ConsoleNavigator.UnSubscribe(currentUser);
                        break;
                    default:
                        Console.WriteLine("Wrong input, please try again");
                        break;
                }
                Console.WriteLine("Would you like to continue? [yes/no]");
                _continue = Console.ReadLine();
            }
        }
    }
}