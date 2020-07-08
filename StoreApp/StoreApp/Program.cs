using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StoreApp.DataAccess.Model;
using StoreApp.DataAccess.Repos;
using StoreApp.Library.Interfaces;
using StoreApp.Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreApp.App
{
    public class Program
    {
        //REQUIREMENTS:
        //place orders to store locations for customers
        //add a new customer
        //search customers by name
        //display details of an order
        //display all order history of a store location
        //display all order history of a customer
        public static readonly DbContextOptions<_2006StoreApplicationContext> Options = new DbContextOptionsBuilder<_2006StoreApplicationContext>()
            .UseSqlServer(SecretConfiguration.ConnectionString)
            .Options;
        static void Main(string[] args)
        {
            
            using var context = new _2006StoreApplicationContext(Options);
            ILocationRepository locationRepo = new LocationRepository(context);
            ICustomerRepository customerRepo = new CustomerRepository(context);
            IOrderRepository orderRepo = new OrderRepository(context);
            //IProductRepository inventoryRepo = new LocationRepository(context);
            //IProductRepository orderLineRepo = new OrderRepository(context);
            Customer currentCustomer = new Customer();
            bool quit = false;
            Console.WriteLine("Welcome to the Store Application");
                string homeMenu =
                    "What would you like to do:\n" +
                    "1: Register as a new customer or sign in\n" +
                    "2: Create new store\n" +
                    "3: Create new product\n" +
                    "4: Add products to a store\n" +
                    "5. Place an order\n" +
                    "6: Search for customer by username\n" +
                    "7: Display details of an order\n" +
                    "8: Display order history of a customer\n" +
                    "9: Display order history of a store\n" +
                    "10: Delete store\n" +
                    "11: Unregister Customer\n" +
                    "0: Quit";


            while (!quit)
            {
                Console.WriteLine(homeMenu);
                Console.WriteLine("Enter your selection(0-11):");
                string userIn = Console.ReadLine();
                int input;
                while (!int.TryParse(userIn, out input))
                {
                    Console.WriteLine("Invalid input. Not a number.");
                    Console.WriteLine("Enter your selection(0-11):");
                    userIn = Console.ReadLine();
                }

                switch (input)
                {
                    case 1:
                        //Register Customers
                        Console.WriteLine("Enter first name");
                        string fn = Console.ReadLine();
                        Console.WriteLine("Enter last name");
                        string ln= Console.ReadLine();
                        Console.WriteLine("Enter user name");
                        string un = Console.ReadLine();
                        Customer newC = new Customer
                        {
                            FirstName = fn,
                            LastName = ln,
                            UserName = un
                        };
                        customerRepo.Register(newC);
                        currentCustomer = customerRepo.GetByUsername(un);
                        var customers = customerRepo.GetAll();
                        foreach(var c in customers)
                        {
                            Console.WriteLine($"{c.CustomerId} {c.FirstName} {c.LastName} {c.UserName}");
                        }
                        break;
                    case 2:
                        //Add Stores
                        
                        break;
                    case 3:
                        //Add Products
                        
                        break;
                    case 4:
                        //Add Products to Stores
                    
                        break;
                    case 5:
                        //Place an order
                        foreach(var s in locationRepo.GetAll())
                        {
                            Console.WriteLine($"{s.Name}\n");
                        }
                        Console.WriteLine("Enter store id to order from:");
                        int sid = Int32.Parse(Console.ReadLine());
                        var store = locationRepo.GetById(sid);
                        //store.Inventory = inventoryRepo.GetAllProducts(store.StoreId);
                        OrderService os = new OrderService(orderRepo, locationRepo);
                        ShoppingCart cart = new ShoppingCart(store);
                        while(true)
                        {
                            
                            foreach (var item in store.Inventory.Keys)
                            {
                                Console.WriteLine($"{item.Name} Stock: {store.Inventory[item]}\n");
                            }
                            Console.WriteLine("Select product id to add to order or 0 to quit:");
                            int pid = Int32.Parse(Console.ReadLine());
                            
                            if(pid == 0)
                            {
                                break;
                            }
                            else
                            {
                                var product = store.Inventory.Keys.First(p => p.ProductId == pid);
                                Console.WriteLine("How much do you want to purchase?");
                                int qty = Int32.Parse(Console.ReadLine());
                                cart.AddToCart(product, qty);
                            }
                        }
                        os.PlaceOrder(cart, currentCustomer);
                        foreach(var order in orderRepo.GetAll())
                        {
                            Console.WriteLine($"{order.OrderId}\n");
                        }
                        break;
                    case 6:
                        //Search customers by username
                     
                        break;
                    case 7:
                        //Get order details for a particular order
                
                        break;
                    case 8:
                        //Check current customer order history
              
                        break;
                    case 9:
                        //Check order history for a particular store
                      

                        break;
                    case 10:
                        //Delete a particular store
               
                        break;
                    case 11:
                        //Delete the current customer
                      
                        break;
                    case 0:
                        quit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter 1-11.");
                        break;
                }
            } 
        }
    }
}
