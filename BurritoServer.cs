using System;
using System.Threading.Tasks;

public class BurritoServer
{
    private readonly int serverId;
    private readonly string serverName;

    private const int TimeforServer1 = 2000;  // Delay of 2000 milliseconds
    private const int TimeforServer2 = 4000;  // Delay of 4000 milliseconds
    private const int TimeforServer3 = 6000;  // Delay of 6000 milliseconds

    public BurritoServer(int id, string server)
    {
        serverId = id;
        serverName = server;
        Console.WriteLine(serverName);
    }

    public Task StartAsync()
    {
        // Run the server loop asynchronously
        return Task.Run(() => RunAsync());
    }

    private async Task RunAsync()
    {
        try
        {
            // The Burrito Server will never stop servicing as long as there are customers
            while (true)
            {
                // Burrito Server waits before checking for customers
                await Task.Delay(1000);

                var currentCustomer = await CustomerHandlingRegistry.GetNextRegisteredCustomerAsync();

                if (currentCustomer != null)
                {
                    var currentOrder = currentCustomer.GetCurrentOrder();

                    currentCustomer.servingServerName = serverName;

                    Console.WriteLine(
                        $"Server: \"{serverName}\" servicing Customer: \"{currentCustomer.GetCustId()}\" " +
                        $"with order of {currentOrder} burrito(s)\n");

                    Logging.LogMatrices("Customer:  " + currentCustomer.GetCustId() + "| " + "Served by : " + serverName);

                    int delay;
                    switch (serverId)
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
                        default:
                            delay = 1000;
                            break;
                    }

                    // Assuming serving time = delay * number of burritos
                    await Task.Delay(currentOrder * delay);

                    Console.WriteLine(
                        $"Server: \"{serverName}\" ---> Customer: \"{currentCustomer.GetCustId()}\". " +
                        $"Time spent servicing: {currentOrder * delay} milliseconds\n");

                    Logging.LogMatrices("Customer:  " + currentCustomer.GetCustId() + "| " + "Served by : " + serverName + $"| Time Spent : { currentOrder* delay} msec");

                    // Update customer order asynchronously
                    await currentCustomer.UpdateBurritoCustomerOrderAsync(currentOrder);
                }
            }
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }
}
