using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;

public class BurritoCustomer
{
    private readonly string customerId;
    private readonly string customerName;
    //private string servingServerName;
    private int toBeServiced, curOrder;
    private int order;
    private readonly List<string> serversServedBy = new List<string>();
    public DateTime StartTime { get; private set; }
    public DateTime StopTime { get; private set; }

    public string servingServerName { get; set; }


    public BurritoCustomer(string id)
    {
        customerId = id;
        customerName = "CustomerName" + id;
        StartTime = DateTime.Now;
        Console.WriteLine("Customer Name:  " + customerName + "\t\t" + "Customer ID:  " + customerId + " - Created");        
    }

    public void SetOrder(int orderNum)
    {
        order = orderNum;
        toBeServiced = order;
    }
    public int GetCurrentOrder() => curOrder;

    public string GetCustId() => customerId;

    public async Task UpdateBurritoCustomerOrderAsync(int orderCount)
    {
        try
        {
            serversServedBy.Add(servingServerName);

            toBeServiced -= orderCount;

            Console.WriteLine($"Customer : {customerId} is serviced {orderCount} burrito(s)\n");
            Logging.LogMatrices($"Customer: {customerId} is serviced {orderCount} burrito(s)  Server:" + servingServerName);

            curOrder = 0;

            if (toBeServiced > 0)
            {
                curOrder = await CustomerHandlingRegistry.RegisterCustomerAsync(this);

                if (curOrder > 1)
                {
                    Console.WriteLine($"Customer : {customerId} is waiting in the WAITING AREA to be serviced.\n");
                    Logging.LogMatrices($"Customer: {customerId}  in WAITING AREA");
                }
            }
            else
            {
                Console.WriteLine($"Customer : {customerId} is paying the server for the {order} burrito(s)\n");
                Logging.LogMatrices($"Customer: {customerId} is PAYING for the Server");

                await Task.Delay(1000);

                Console.WriteLine($"Customer : {customerId} is leaving the Restaurant after being serviced with {order} burrito(s)\n");
                Logging.LogMatrices($"Customer: {customerId} Service Complete - LEAVING RESTURANT");

                CustomerServiceComplete();
            }

            await Task.Delay(1000);
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }

    public int CustomerToBeServiced() => toBeServiced;

    public async Task RunAsync()
    {
        try
        {
            Logging.LogMatrices("Customer:  " + customerId + "| " + " BurritoCount: " + order.ToString());

            if (CustomerHandlingRegistry.IsThereRoomForCustomerToWait())
            {
                Console.WriteLine($"The Customer : {customerId} is being registered\n");
                Logging.LogMatrices("Customer:  " + customerId + "| " + "Will be Served");

                curOrder = await CustomerHandlingRegistry.RegisterCustomerAsync(this);
            }
            else
            {
                Console.WriteLine(
                    $"THE RESTAURANT IS FULL!\n\tThe Customer: {this.GetCustId()} will not be serviced\n\t" +
                    $"The Customer will not be entered in the Waiting Area\n\tSorry for the inconvenience caused!\n");

                string logString = $"\n------------\nTHE RESTAURANT IS FULL!\n\tThe Customer: {this.GetCustId()} will not be serviced\n\t" + $"The Customer will not be entered in the Waiting Area\n\tSorry for the inconvenience caused!\n------------\n";
                
                Logging.LogFile(logString);
                Logging.LogMatrices("Customer:  "  + customerId + "| " + "Not Served");
            }
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }

        await Task.CompletedTask;
    }

    public Task StartAsync()
    {
        // Start customer lifecycle asynchronously
        return Task.Run(() => RunAsync());
    }

    public void CustomerServiceComplete()
    {
        try
        {
            StopTime = DateTime.Now;
            // Calculate elapsed time
            TimeSpan elapsed = StopTime - StartTime;

            string customer = customerId;
            string noOfBurritos = order.ToString();
            string servers = string.Join(", ", serversServedBy);
            string startTime = StartTime.ToString();
            string stopTime = StopTime.ToString();
            string numberOfBatches = serversServedBy.Count.ToString();
            string totalTimeElapsed = elapsed.TotalSeconds.ToString() + "seconds";

            string logString = $"\nCustomer : {customerName}  |  Order : {noOfBurritos}  | Server : {servers} | Batches: {numberOfBatches} | StartTime : {startTime} | StopTime: {stopTime} | TotalTime: {totalTimeElapsed} \n------------\n";

            Logging.LogFile(logString);
            Logging.LogMatrices(logString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
