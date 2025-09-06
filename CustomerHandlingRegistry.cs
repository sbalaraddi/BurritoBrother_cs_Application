using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public static class CustomerHandlingRegistry
{
    private static readonly SemaphoreSlim available = new SemaphoreSlim(1, 1);

    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerOneBurrito = new List<BurritoCustomer>();
    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerTwoBurrito = new List<BurritoCustomer>();
    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerThreeBurrito = new List<BurritoCustomer>();

    public static bool IsThereRoomForCustomerToWait()
    {
        int totalCustomersWaiting = RegisteredBurritoCustomerOneBurrito.Count +
                RegisteredBurritoCustomerTwoBurrito.Count +
                RegisteredBurritoCustomerThreeBurrito.Count;

        return (totalCustomersWaiting <= 14);
    }

    public static async Task<int> RegisterCustomerAsync(BurritoCustomer newCustomer)
    {
        await available.WaitAsync();
        try
        {
            int orderToService = newCustomer.CustomerToBeServiced();

            if (orderToService > 0)
            {
                switch (orderToService)
                {
                    case 1:
                        RegisteredBurritoCustomerOneBurrito.Add(newCustomer);
                        break;
                    case 2:
                        RegisteredBurritoCustomerTwoBurrito.Add(newCustomer);
                        break;
                    default:
                        orderToService = 3;
                        RegisteredBurritoCustomerThreeBurrito.Add(newCustomer);
                        break;
                }
            }
            //else
            //{
            //    Console.WriteLine(
            //        $"THE RESTAURANT IS FULL!\n\tThe Customer: {newCustomer.GetCustId()} will not be serviced\n\tSorry for the inconvenience caused!\n");
            //}

            return orderToService;
        }
        finally
        {
            available.Release();
        }
    }

    public static async Task<BurritoCustomer> GetNextRegisteredCustomerAsync()
    {
        await available.WaitAsync();
        try
        {
            BurritoCustomer customerToService = null;

            if (RegisteredBurritoCustomerOneBurrito.Count > 0)
            {
                customerToService = RegisteredBurritoCustomerOneBurrito[0];
                RegisteredBurritoCustomerOneBurrito.RemoveAt(0);
            }
            else if (RegisteredBurritoCustomerTwoBurrito.Count > 0)
            {
                customerToService = RegisteredBurritoCustomerTwoBurrito[0];
                RegisteredBurritoCustomerTwoBurrito.RemoveAt(0);
            }
            else if (RegisteredBurritoCustomerThreeBurrito.Count > 0)
            {
                customerToService = RegisteredBurritoCustomerThreeBurrito[0];
                RegisteredBurritoCustomerThreeBurrito.RemoveAt(0);
            }

            return customerToService;
        }
        finally
        {
            available.Release();
        }
    }
}
