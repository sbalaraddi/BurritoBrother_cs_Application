using System;
using System.Threading;

public class BurritoServer
{
    private readonly int myServerId;
    private readonly string myServerName;

    private const int TimeforServer1 = 2000;  // Delay of 2000 milliseconds
    private const int TimeforServer2 = 4000;  // Delay of 4000 milliseconds
    private const int TimeforServer3 = 6000;  // Delay of 6000 milliseconds

    private Thread thread;

    public BurritoServer(int id, string serverName)
    {
        myServerId = id;
        myServerName = serverName;
        Console.WriteLine(myServerName);

        // Each server runs on its own thread
        thread = new Thread(Run);
    }

    public void Start()
    {
        thread.Start();
    }

    private void Run()
    {
        try
        {
            // The Burrito Server will never stop servicing as long as there are customers
            while (true)
            {
                // Burrito Server thread waits before servicing customers
                Thread.Sleep(1000);

                BurritoCustomer currentCustomer = CustomerHandlingRegistry.GetNextRegisteredCustomer();

                if (currentCustomer != null)
                {
                    int currentOrder = currentCustomer.GetCurrentOrder();
                    Console.WriteLine(
                        $"Server ID: \"{myServerName}\" servicing customer, " +
                        $"Customer ID: \"{currentCustomer.GetCustId()}\" of order {currentOrder} burritos\n");

                    int delay = 1000;
                    switch (myServerId)
                    {
                        case 1:
                            delay = TimeforServer1;
                            break;
                        case 2:
                            delay = TimeforServer2;
                            break;
                        case 3:
                            delay = TimeforServer3;
                            break;
                    }

                    // Assuming serving time = delay * number of burritos
                    Thread.Sleep(currentOrder * delay);

                    Console.WriteLine(
                        $"Server ID: \"{myServerName}\" ---> Customer ID: \"{currentCustomer.GetCustId()}\". " +
                        $"Time Spent in servicing the customer : {currentOrder * delay} milliseconds\n");

                    currentCustomer.UpdateBurritoCustomerOrder(currentCustomer.GetCurrentOrder());
                }
            }
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }
}
