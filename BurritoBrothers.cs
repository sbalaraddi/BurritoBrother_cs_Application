using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class BurritoBrothers
{
    // Async Entry Point 
    public static async Task Main(string[] args)
    {
        try
        {
            Console.WriteLine(
                "\n\n\t\t\tWELCOME TO BURRITO BROTHERS\n" +
                "\t\t\tWE OFFER VERY TASTY BURRITO IN THE TOWN\n\n\n");

            // Create Burrito Servers
            var servers = new List<BurritoServer>
            {
                new BurritoServer(1, "Burrito Server 1"),
                new BurritoServer(2, "Burrito Server 2"),
                new BurritoServer(3, "Burrito Server 3")
            };

            // Start servers asynchronously
            var serverTasks = new List<Task>();
            foreach (var server in servers)
            {
                serverTasks.Add(server.StartAsync());
            }

            // Create Burrito Customer List
            var burritoCustomerList = new List<BurritoCustomer>();

            Console.WriteLine("\n\nThe Burrito Servers start servicing the customers entering the BURRITO BROTHERS Restaurant!\n\n");

            var random = new Random();
            var i = 0;

            while (true)
            {
                // Control customer flow every 5 seconds
                await Task.Delay(5000);

                // Generate the Customer Id and the Customer String 
                i++;
                var customerString = $"Burrito Customer {i}";

                // Generate random burrito orders between 1 and 20
                var randomOrder = random.Next(1, 21);

                if (randomOrder < 1 || randomOrder > 20)
                {
                    Console.WriteLine($"Customer {customerString} is not allowed (invalid order: {randomOrder})\n");
                    return;
                }
                else
                {
                    // Valid burrito order → create BurritoCustomer
                    var newCustomer = new BurritoCustomer(customerString);
                    newCustomer.SetOrder(randomOrder);

                    Console.WriteLine(
                        $"Customer: \"{customerString}\" entering restaurant with order of {randomOrder} burrito(s)\n");

                    // Start customer asynchronously
                    _ = newCustomer.StartAsync(); // fire-and-forget (customer lifecycle)

                    burritoCustomerList.Add(newCustomer);

                    await Task.Delay(1000);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error: {ex.Message}");
        }
    }
}