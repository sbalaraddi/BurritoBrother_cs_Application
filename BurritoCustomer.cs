using System;
using System.Threading;

public class BurritoCustomer
{
    private readonly string myCustomerId;
    private bool myCustomerServiceComplete = false;
    private int myToBeServiced, myCurOrder;
    private int myOrder;
    private long startTime;
    private long endTime;
    private long waitTime;

    private Thread thread;

    public BurritoCustomer(string id)
    {
        myCustomerId = id;
        Console.WriteLine(myCustomerId);

        // Each customer runs on its own thread"
        thread = new Thread(Run);
    }

    public void SetOrder(int orderNum)
    {
        myOrder = orderNum;
        myToBeServiced = myOrder;
    }

    public int GetCurrentOrder()
    {
        return myCurOrder;
    }

    public string GetCustId()
    {
        return myCustomerId;
    }

    public void UpdateBurritoCustomerOrder(int orderCount)
    {
        try
        {
            myToBeServiced -= orderCount;

            Console.WriteLine($"Customer : {myCustomerId} is Serviced {orderCount} Burritos \n");
            myCurOrder = 0;

            if (myToBeServiced > 0)
            {
                myCurOrder = CustomerHandlingRegistry.RegisterCustomer(this);

                if (myCurOrder > 1)
                {
                    Console.WriteLine($"Customer : {myCustomerId} is waiting in the WAITING AREA to be serviced.\n");
                }
            }
            else
            {
                Console.WriteLine($"Customer : {myCustomerId} is paying the server for the {myOrder} burritos \n");

                Thread.Sleep(1000);

                Console.WriteLine($"Customer : {myCustomerId} is leaving the Restaurant after being serviced with {myOrder} burritos \n");

                CustomerServiceComplete();
            }

            Thread.Sleep(1000);
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }

    public int CustomerToBeServiced()
    {
        return myToBeServiced;
    }

    public void Run()
    {
        try
        {
            if (CustomerHandlingRegistry.IsThereRoomForCustomerToWait() == true)
            {
                Console.WriteLine($"The Customer : {myCustomerId} is being Registered \n");

                myCurOrder = CustomerHandlingRegistry.RegisterCustomer(this);
            }
            else
            {
                Console.WriteLine(
                    $"THE RESTAURANT IS FULL!\n\tThe Customer: {this.GetCustId()} will not be serviced\n\tThe Customer will not be entered in the Waiting Area\n\tSorry for the inconvenience caused!\n");
            }
        }
        catch (Exception e1)
        {
            Console.WriteLine(e1);
        }
    }

    public void Start()
    {
        thread.Start();
    }

    public void CustomerServiceComplete()
    {
        try
        {
            myCustomerServiceComplete = true;

            string customer = myCustomerId;
            string noOfBurritos = myOrder.ToString();
            string server = "Server Name";
            string waitingTime = "00";
            string numberOfBatches = "1";
            string totalTimeInRes = "123";

            string logString = customer + "    " + noOfBurritos + "    " +
                               server + "    " + waitingTime + "    " +
                               numberOfBatches + "    " + totalTimeInRes;

            Logging.LogFile(logString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
