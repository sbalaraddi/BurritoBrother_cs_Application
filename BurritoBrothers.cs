using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

class BurritoBrothers
{
    // Entry point
    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("\n\n\t\t\tWELCOME TO BURRITO BROTHERS\n\n\t\t\tWE OFFER VERY TASTY BURRITO IN THE TOWN\n\n\n\n");

            // Create three Burrito Servers
            string serverName1 = "Burrito Server 1";
            int serverID1 = 1;
            BurritoServer server1 = new BurritoServer(serverID1, serverName1);
            server1.Start();

            string serverName2 = "Burrito Server 2";
            int serverID2 = 2;
            BurritoServer server2 = new BurritoServer(serverID2, serverName2);
            server2.Start();

            string serverName3 = "Burrito Server 3";
            int serverID3 = 3;
            BurritoServer server3 = new BurritoServer(serverID3, serverName3);
            server3.Start();

            // Create Burrito Customer List
            List<BurritoCustomer> burritoCustomerList = new List<BurritoCustomer>();

            Console.WriteLine("\n\nThe Burrito Servers start servicing the customers entering the BURRITO BROTHERS Restaurant!\n\n");

            int i = 0;
            Random random = new Random();

            while (true)
            {
                // Control customer flow
                Thread.Sleep(5000);

                // Generate the Customer Id and the Customer String 
                i++;
                string customerId = i.ToString();
                string customerString = "Burrito Customer " + customerId;

                // Generate random burrito orders between 1 and 20
                int min = 1;
                int max = 20;
                int randomNumber = random.Next(min, max + 1);

                if (randomNumber < 1 || randomNumber > 20)
                {
                    Console.WriteLine($"Customer {customerString} is not allowed as the burrito order is not valid\n");
                    Environment.Exit(0);
                }
                else
                {
                    // If the burrito order is valid, create a BurritoCustomer thread
                    BurritoCustomer newCustomer = new BurritoCustomer(customerString);
                    newCustomer.SetOrder(randomNumber);

                    Console.WriteLine($"Customer ID: \"{customerString}\" entering restaurant with order of {randomNumber} Burritos\n");

                    newCustomer.Start();
                    Thread.Sleep(1000);

                    burritoCustomerList.Add(newCustomer);
                }
            }
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }
}