using System;
using System.Collections.Generic;
using System.Threading;

public static class CustomerHandlingRegistry
{
    private static SemaphoreSlim available = new SemaphoreSlim(1, 1);

    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerOneBurrito = new List<BurritoCustomer>();
    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerTwoBurrito = new List<BurritoCustomer>();
    private static readonly List<BurritoCustomer> RegisteredBurritoCustomerThreeBurrito = new List<BurritoCustomer>();

    public static int RegisterCustomer(BurritoCustomer newCustomer)
    {
        available.Wait();
        try
        {
            int orderToService = newCustomer.CustomerToBeServiced();

            if ((RegisteredBurritoCustomerOneBurrito.Count +
                 RegisteredBurritoCustomerTwoBurrito.Count +
                 RegisteredBurritoCustomerThreeBurrito.Count) > 14)
            {
                orderToService = 0;
            }

            //This will ensure each customer will be served the max of 3 burritos order at a time(Round Robin)
            if (orderToService > 0)
            {
                if (orderToService == 1)
                {
                    RegisteredBurritoCustomerOneBurrito.Add(newCustomer);
                }
                else if (orderToService == 2)
                {
                    RegisteredBurritoCustomerTwoBurrito.Add(newCustomer);
                }
                else
                {
                    orderToService = 3;
                    RegisteredBurritoCustomerThreeBurrito.Add(newCustomer);
                }
            }
            else
            {
                Console.WriteLine(
                    $"THE RESTAURANT IS FULL!\n\tThe Customer: {newCustomer.GetCustId()} will not be serviced\n\tSorry for the inconvenience caused!\n");
            }

            return orderToService;
        }
        finally
        {
            available.Release();
        }
    }

    public static BurritoCustomer GetNextRegisteredCustomer()
    {
        available.Wait();
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
