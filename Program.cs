
using System;
using System.Collections.Generic;

// Main Application Class
public class TableReservationApp
{
    static void Main(string[] args)
    {
        ReservationManagerClass m = new ReservationManagerClass();
        m.AddRestaurantMethod("A", 10);
        m.AddRestaurantMethod("B", 5);

        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // True
        Console.WriteLine(m.BookTable("A", new DateTime(2023, 12, 25), 3)); // False
    }
}

// Reservation Manager Class
public class ReservationManagerClass
{
    // res
    public List<RestaurantClass> restaurants;

    public ReservationManagerClass()
    {
        restaurants = new List<RestaurantClass>();
    }

    // Add Restaurant Method
    public void AddRestaurantMethod(string name, int tables)
    {
        try
        {
            RestaurantClass restaurants = new RestaurantClass();
            restaurants.n = name;
            restaurants.t = new RestaurantTableClass[tables];
            for (int i = 0; i < tables; i++)
            {
                restaurants.t[i] = new RestaurantTableClass();
            }
            this.restaurants.Add(restaurants);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // Load Restaurants From
    // File
    private void LoadRestaurantsFromFileMethod(string fileP)
    {
        try
        {
            string[] ls = File.ReadAllLines(fileP);
            foreach (string l in ls)
            {
                var parts = l.Split(',');
                if (parts.Length == 2 && int.TryParse(parts[1], out int tableCount))
                {
                    AddRestaurantMethod(parts[0], tableCount);
                }
                else
                {
                    Console.WriteLine(l);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    //Find All Free Tables
    public List<string> FindAllFreeTables(DateTime dt)
    {
        try
        { 
            List<string> free = new List<string>();
            foreach (var r in restaurants)
            {
                for (int i = 0; i < r.t.Length; i++)
                {
                    if (!r.t[i].IsBooked(dt))
                    {
                        free.Add($"{r.n} - Table {i + 1}");
                    }
                }
            }
            return free;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return new List<string>();
        }
    }

    public bool BookTable(string rName, DateTime d, int tableNumber)
    {
        foreach (var r in restaurants)
        {
            if (r.n == rName)
            {
                if (tableNumber < 0 || tableNumber >= r.t.Length)
                {
                    throw new Exception(null); //Invalid table number
                }

                return r.t[tableNumber].Book(d);
            }
        }

        throw new Exception(null); //Restaurant not found
    }

    public void SortRestaurantsByAvailabilityForUsersMethod(DateTime dateTime)
    {
        try
        { 
            bool swapped;
            do
            {
                swapped = false;
                for (int i = 0; i < restaurants.Count - 1; i++)
                {
                    int availableTablesCountCurrent =  CountAvailableTables(restaurants[i], dateTime); // available tables current
                    int availableTablesCountNext =  CountAvailableTables(restaurants[i + 1], dateTime); // available tables next

                    if (availableTablesCountCurrent < avTn)
                    {
                        // Swap restaurants
                        var temporaryRestaurant = restaurants[i];
                        restaurants[i] = restaurants[i + 1];
                        restaurants[i + 1] = temporaryRestaurant;
                        swapped = true;
                    }
                }
            } while (swapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
        }
    }

    // count available tables in a restaurant
    public int  CountAvailableTables(RestaurantClass r, DateTime dt)
    {
        try
        {
            int count = 0;
            foreach (var t in r.t)
            {
                if (!t.IsBooked(dt))
                {
                    count++;
                }
            }
            return count;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return 0;
        }
    }
}

// Restaurant Class
public class RestaurantClass
{
    public string n; //name
    public RestaurantTableClass[] t; // tables
}

// Table Class
public class RestaurantTableClass
{
    private List<DateTime> bd; //booked dates


    public RestaurantTableClass()
    {
        bd = new List<DateTime>();
    }

    // book
    public bool Book(DateTime d)
    {
        try
        { 
            if (bd.Contains(d))
            {
                return false;
            }
            //add to bd
            bd.Add(d);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error");
            return false;
        }
    }

    // is booked
    public bool IsBooked(DateTime d)
    {
        return bd.Contains(d);
    }
}
