using System;
using System.Linq;
using Library.TaskManagement.Models;
using Library.TaskManagement.Services;
using Newtonsoft.Json;

namespace MyApp // Note: actual namespace depends on the project name.
{
    internal class Program
    {

        
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Task Management App for 2022!");
            //ItemService CartObject = new ItemService();
            //ItemService InventoryObject = new ItemService();

            //neeeed to make thesee two ddifferent currents

            var Inventory = ItemService.Current;
            var Cart = CartService.Current;

            bool mainLoop = true;

            while (mainLoop)
            {
                Console.WriteLine("Would you like to access the inventory or the cart?");
                Console.WriteLine("1. Access the cart:");
                Console.WriteLine("2. Access the inventory:");
                Console.WriteLine("3. Exit the program:");
                var input = Console.ReadLine() ?? string.Empty;

                if (input == "1")
                {
                    CartMenu(Cart, Inventory);
                }
                else if (input == "2")
                {
                    InventoryMenu(Inventory);
                }
                else if(input == "3")
                {
                    mainLoop = false;
                }
                else
                {
                    Console.WriteLine("Invalid selection...");
                }
            }

            Console.WriteLine("Thank you for using the Task Management App!");
        }




        public static void CartMenu(CartService Cart, ItemService Inventory)
        {
            bool CartLoop = true;
            while (CartLoop)
            {
                var action = PrintCartMenu();


                if (action == ActionType.Create)
                {
                    int productNumber = 0;
                    Console.WriteLine("You have chosen to add a product to your cart.");
                    Console.WriteLine("Choose a product from the inventory based upon it's id.");
                    int id;
                    var stringId = (Console.ReadLine() ?? "0");
                    if((int.TryParse(stringId, out id)== false))
                      {
                        Console.WriteLine("You entered an invalid input.");
                        return;
                    }
                    for (int i = 0; i < Inventory.itemList.Count(); i++)
                    {
                        if (id == Inventory.itemList[i].Id)
                        {
                            productNumber = i;
                            Console.WriteLine($"How many {Inventory.Items[productNumber].Name}s would you like to add to your cart?");
                            var stringNumAdded = (Console.ReadLine() ?? "0");
                            int numAdded;
                            if((int.TryParse(stringNumAdded, out numAdded) == false)){
                                Console.WriteLine("You entered an invalid input.");
                                return;
                            }
                            Cart.Create(productNumber, numAdded, Inventory);
                        }
                        else
                        {
                            //Console.WriteLine("No item associated with that id.");

                        }
                    }




                }
                else if(action == ActionType.Checkout)
                {
                    Cart.Checkout();
                }
                else if (action == ActionType.ReadCart)
                {
                    Console.WriteLine("You chose to list the Cart:");
                    if (Cart.Items.Count == 0)
                    {
                        Console.WriteLine("Your cart is currently empty.");
                    }
                    else
                    {
                       
                        Cart.Display();
                    }
                }
                else if (action == ActionType.Read)
                {
                    Console.WriteLine("You chose to list the inventory:");
                    if (Inventory.Items.Count == 0)
                    {
                        Console.WriteLine("The Inventory is currently empty.");
                    }
                    else
                    {

                        Inventory.Display();
                    }
                }
                else if (action == ActionType.Sort)
                {
                    Inventory.Sort(Cart);

                }
                else if (action == ActionType.Delete)
                {
                    int productNumber = 0;

                    Console.WriteLine("You chose to remove an item from the cart");
                    Console.WriteLine("Choose a product from the cart based upon it's id.");
                    var stringId = (Console.ReadLine() ?? "0");
                    int id;
                    if ((int.TryParse(stringId, out id) == false))
                    {
                        Console.WriteLine("You entered an invalid input.");
                        return;
                    }
                    for (int i = 0; i < Cart.itemList.Count(); i++)
                    {
                        if (id == Cart.itemList[i].Id)
                        {
                            productNumber = i;
                            Console.WriteLine($"How many {Cart.Items[productNumber].Name}s would you like to remove from your cart?");
                            var numRemoved = int.Parse(Console.ReadLine() ?? "0");
                            Cart.Delete(productNumber, numRemoved, Inventory);
                        }
                        else
                        {
                           // Console.WriteLine("No item associated with that id.");

                        }
                    }
                }
                else if (action == ActionType.Save)
                {

                    Console.WriteLine("Saving items in cart...");
                    Cart.Save("/Users/noahwilliamshaffer/Projects/ProjectExample/CartSave.txt");
                }
                else if (action == ActionType.Load)
                {


                    Console.WriteLine("Loading items into cart...");
                    Cart.Load("/Users/noahwilliamshaffer/Projects/ProjectExample/CartSave.txt");
                }
                else if (action == ActionType.Exit)
                {
                    Console.WriteLine("You chose to exit");
                    CartLoop = false;
                }

            }

        }
        public static void InventoryMenu(ItemService Inventory){
            bool inventoryLoop = true;
            while (inventoryLoop)
            {
                var action = PrintInventoryMenu();


                if (action == ActionType.Create)
                {
                    Console.WriteLine("Would you like to have this product sorted by weight or by quantity?");
                    Console.WriteLine("1. Product by weight");
                    Console.WriteLine("2. Product by Quantity");
                    var typeChoice = int.Parse(Console.ReadLine() ?? "0");
                    Item? newItem = null;
                    if (typeChoice == 1)
                    {
                        newItem = new ProductByWeight();
                        Console.WriteLine("Created new product by weight");
                        AddWeightInventory(Inventory, newItem);
                    }
                    else
                    {
                        newItem = new ProductByQuantity();
                        Console.WriteLine("Created new product by Quantity");
                        AddQuantityInventory(Inventory, newItem);
                    }

                    //AddToInventory(Inventory, newItem);




                }
                else if (action == ActionType.Read)
                {
                    if (Inventory.Items.Count == 0)
                    {
                        Console.WriteLine("The Inventory is currently empty.");
                    }
                    else
                    {
                        
                        Inventory.Display();
                    }
                }
                
                else if (action == ActionType.BOGO)
                {
                    Console.WriteLine("You chose to make an item BOGO");
                    Console.WriteLine("Which task would you like to make BOGO?");
                    var stringChoice = (Console.ReadLine() ?? "0");
                    int choice;
                    if (int.TryParse(stringChoice, out choice) == false)
                    {
                        Console.Write("Invalid choice entered, exiting");
                        return;
                    }
                    var todoOfInterest = Inventory.Items.FirstOrDefault(t => t.Id == choice);
                    Inventory.makeBogo(todoOfInterest);
                }
                
                else if (action == ActionType.Update)
                {
                    Console.WriteLine("You chose to update a task");
                    Console.WriteLine("Which task would you like to update?");
                    var stringChoice = (Console.ReadLine() ?? "0");
                    int choice;
                    if (int.TryParse(stringChoice, out choice) == false)
                    {
                        Console.Write("Invalid choice entered, exiting");
                        return;
                    }
                    var todoOfInterest = Inventory.Items.FirstOrDefault(t => t.Id == choice);
                    //AddToInventory(Inventory,todoOfInterest);
                    Inventory.Update(todoOfInterest);
                }
                else if (action == ActionType.Delete)
                {
                    Console.WriteLine("You chose to delete a task");
                    Console.WriteLine("Which task would you like to delete?");
                    var id = int.Parse(Console.ReadLine() ?? "0");
                    Inventory.Delete(id);
                }
                else if (action == ActionType.Save)
                {
                    Inventory.Save("/Users/noahwilliamshaffer/Projects/ProjectExample/InventorySave.txt");
                }
                else if (action == ActionType.Load)
                {
                    Inventory.Load("/Users/noahwilliamshaffer/Projects/ProjectExample/InventorySave.txt");
                }
                else if (action == ActionType.Exit)
                {
                    Console.WriteLine("You chose to exit");
                    inventoryLoop = false;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a valid Option.");
                  
                }

        }

       

    }
      

        public static void AddWeightInventory(ItemService inventory, Item? item)
        { 
            double numPricePerPound;
            double numWeight;
            

            if (item == null)
            {
                return;
            }
            Console.WriteLine("What is the name for the item?");
            item.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description for the item?");
            item.Description = Console.ReadLine() ?? string.Empty;

            if (item is ProductByWeight)
            {


                var todo = item as ProductByWeight;
                if (todo != null)
                {
                    //Console.WriteLine("The product will be listed by weight.");

                    Console.WriteLine("What is the weight for the item?");
                    var weight = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(weight, out numWeight) == false)
                    {
                        Console.Write("Invalid Quatity entered, exiting");
                        return;
                    }
                    ((ProductByWeight)item).Pounds = numWeight;

                    Console.WriteLine("What is the price per pound?");
                    var price = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(price, out numPricePerPound) == false)
                    {
                        Console.Write("Invalid price entered, exiting");
                        return;
                    }
                    //((ProductByWeight)item).PricePerPound = numPricePerPound;
                    ((ProductByWeight)item).PricePerUnit= numPricePerPound;

                    //((ProductByWeight)item).TotalPrice = ((ProductByWeight)item).PricePerPound * ((ProductByWeight)item).Pounds;
                    ((ProductByWeight)item).TotalPrice = ((ProductByWeight)item).PricePerUnit * ((ProductByWeight)item).Pounds;

                    for (int i = 0; i < inventory.itemList.Count(); i++)
                    {
                        if (inventory.itemList[i].Name == item.Name)
                        {
                            Console.WriteLine("Item already in inventory");
                            if (inventory.itemList[i] is ProductByWeight && item is ProductByWeight)
                            {
                                Console.WriteLine("Weight");
                                ((ProductByWeight)inventory.itemList[i]).Pounds = ((ProductByWeight)inventory.itemList[i]).Pounds + ((ProductByWeight)item).Pounds;
                                //Console.WriteLine($"pounds{((ProductByWeight)item).Pounds} * pricePerPound(inventory){((ProductByWeight)inventory.itemList[i]).PricePerPound}");
                                Console.WriteLine($"pounds{((ProductByWeight)item).Pounds} * pricePerPound(inventory){((ProductByWeight)inventory.itemList[i]).PricePerUnit}");
                                //((ProductByWeight)inventory.itemList[i]).TotalPrice = ((ProductByWeight)inventory.itemList[i]).Pounds * ((ProductByWeight)inventory.itemList[i]).PricePerPound;
                                ((ProductByWeight)inventory.itemList[i]).TotalPrice = ((ProductByWeight)inventory.itemList[i]).Pounds * ((ProductByWeight)inventory.itemList[i]).PricePerUnit;
                            }

                            return;
                        }


                    }
                }
            }
            inventory.Create(item);
        }

        public static void AddQuantityInventory(ItemService inventory, Item? item)
        {
            double numPrice;
            int numQuantity;


            if (item == null)
            {
                return;
            }
            Console.WriteLine("What is the name for the item?");
            item.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description for the item?");
            item.Description = Console.ReadLine() ?? string.Empty;

            var appointment = item as ProductByQuantity;
            if (appointment != null)
            {
                //Console.WriteLine("The product will be listed by quantity.");

                Console.WriteLine("What is the quantity for the item?");
                var quantity = Console.ReadLine() ?? string.Empty;
                
                
                if (Int32.TryParse(quantity, out numQuantity) == false)
                {
                    Console.Write("Invalid Quatity entered, exiting");
                    return;
                }

                ((ProductByQuantity)item).Quantity = numQuantity;

                Console.WriteLine("What is the price per item?");
                var price = Console.ReadLine() ?? string.Empty;
                if (double.TryParse(price, out numPrice) == false)
                {
                    Console.Write("Invalid price entered, exiting");
                    return;
                }
                ((ProductByQuantity)item).PricePerUnit = numPrice;

                ((ProductByQuantity)item).TotalPrice = numPrice * numQuantity;


                for (int i = 0; i < inventory.itemList.Count(); i++)
                {
                    if (inventory.itemList[i].Name == item.Name)
                    {
                        Console.WriteLine("Item already in inventory");
                        if (inventory.itemList[i] is ProductByQuantity && item is ProductByQuantity)
                        {
                            Console.WriteLine("Weight");
                            ((ProductByQuantity)inventory.itemList[i]).Quantity += ((ProductByQuantity)item).Quantity;
                            ((ProductByQuantity)inventory.itemList[i]).TotalPrice = ((ProductByQuantity)inventory.itemList[i]).Quantity * ((ProductByQuantity)inventory.itemList[i]).PricePerUnit;
                            return;
                        }
          

                        
                    }


                }


            }
            inventory.Create(item);

        }
    


        public static void AddToInventory(ItemService inventory, Item? item)
        {
            
            double numPrice;
            double numPricePerPound;
            double numWeight;
            int numQuantity;

            if (item == null)
            {
                return;
            }
            Console.WriteLine("What is the name for the item?");
            item.Name = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("What is the description for the item?");
            item.Description = Console.ReadLine() ?? string.Empty;
                var todo = item as ProductByWeight;
                if (todo != null)
                {
                   // Console.WriteLine("The product will be listed by weight.");

                    Console.WriteLine("How many pound of inventory are you adding?");
                    var weight = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(weight, out numWeight) == false)
                    {
                        Console.Write("Invalid Quatity entered, exiting");
                        return;
                    }
                    ((ProductByWeight)item).Pounds = numWeight;

                    Console.WriteLine("What is the price per pound?");
                    var price = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(price, out numPricePerPound) == false)
                    {
                        Console.Write("Invalid price entered, exiting");
                        return;
                    }
                    //((ProductByWeight)item).PricePerPound = numPricePerPound;
                ((ProductByWeight)item).PricePerUnit = numPricePerPound;

                //((ProductByWeight)item).TotalPrice = ((ProductByWeight)item).PricePerPound * ((ProductByWeight)item).Pounds;
                ((ProductByWeight)item).TotalPrice = ((ProductByWeight)item).PricePerUnit * ((ProductByWeight)item).Pounds;

                for (int i = 0; i < inventory.itemList.Count(); i++)
                    {
                        if (inventory.itemList[i].Name == item.Name)
                        {
                            Console.WriteLine("Item already in inventory");
                            if (inventory.itemList[i] is ProductByWeight && item is ProductByWeight)
                            {
                                Console.WriteLine("Weight");
                                ((ProductByWeight)inventory.itemList[i]).Pounds += ((ProductByWeight)item).Pounds;
                                //((ProductByWeight)inventory.itemList[i]).TotalPrice = ((ProductByWeight)inventory.itemList[i]).Pounds * ((ProductByWeight)inventory.itemList[i]).PricePerPound;
                                ((ProductByWeight)inventory.itemList[i]).TotalPrice = ((ProductByWeight)inventory.itemList[i]).Pounds * ((ProductByWeight)inventory.itemList[i]).PricePerUnit;
                            return;
                            }
                           

                            
                        }


                    }
              
            }
            else if (item is ProductByQuantity)
            {

                var appointment = item as ProductByQuantity;
                if (appointment != null)
                {
                    Console.WriteLine("The product will be listed by quantity.");

                    Console.WriteLine("What is the quantity for the item?");
                    var quantity = Console.ReadLine() ?? string.Empty;
                    if (Int32.TryParse(quantity, out numQuantity) == false)
                    {
                        Console.Write("Invalid Quatity entered, exiting");
                        return;
                    }

                    ((ProductByQuantity)item).Quantity = numQuantity;

                    Console.WriteLine("What is the price per item?");
                    var price = Console.ReadLine() ?? string.Empty;
                    if (double.TryParse(price, out numPrice) == false)
                    {
                        Console.Write("Invalid price entered, exiting");
                        return;
                    }
                    ((ProductByQuantity)item).PricePerUnit = numPrice;

                    ((ProductByQuantity)item).TotalPrice = numPrice * numQuantity;


                    for (int i = 0; i < inventory.itemList.Count(); i++)
                    {
                        if (inventory.itemList[i].Name == item.Name)
                        {
                            Console.WriteLine("Item already in inventory");
                            if (inventory.itemList[i] is ProductByQuantity && item is ProductByQuantity)
                            {
                                Console.WriteLine("Weight");
                                ((ProductByQuantity)inventory.itemList[i]).Quantity += ((ProductByQuantity)item).Quantity;
                                ((ProductByQuantity)inventory.itemList[i]).TotalPrice = ((ProductByQuantity)item).Quantity * ((ProductByQuantity)item).PricePerUnit;
                            }
                            /*
                            if (inventory.itemList[i] is ProductByQuantity && item is ProductByQuantity)
                            {
                                Console.WriteLine("Quantity");
                                ((ProductByQuantity)inventory.itemList[i]).Quantity += ((ProductByQuantity)item).Quantity;

                            }
                            */

                            return;
                        }


                    }


                }
                inventory.Create(item);
            }
            /*
                for (int i = 0; i < inventory.itemList.Count(); i++)
                {
                    if (inventory.itemList[i].Name == item.Name)
                    {
                        Console.WriteLine("Item already in inventory");
                    if (inventory.itemList[i] is ProductByWeight && item is ProductByWeight)
                    {
                        Console.WriteLine("Weight");
                        ((ProductByQunatity)inventory.itemList[i].Pounds += item.Pounds;
                    }
                    if (inventory.itemList[i] is ProductByQuantity && item is ProductByQuantity)
                    {
                        Console.WriteLine("Quantity");
                        inventory.itemList[i].Quantity += item.Quantity;

                    }

                        return;
                    }
            

                }
            */
            inventory.Create(item);

        }

            public static ActionType PrintCartMenu()
        {
            //CRUD = Create, Read, Update, and Delete
            

            while (true)
            {
                Console.WriteLine("Select an option to begin:");
                Console.WriteLine("1. Add a the cart");
                Console.WriteLine("2. Delete from the cart");
                Console.WriteLine("3. List Cart");
                Console.WriteLine("4. List Inventory");
                Console.WriteLine("5. Sort Options");
                Console.WriteLine("6. Checkout");
                Console.WriteLine("7. Save Cart");
                Console.WriteLine("8. Load Cart");
                Console.WriteLine("9. Exit Cart");

                var stringInput = (Console.ReadLine() ?? "0");
                int input;
                if ((int.TryParse(stringInput, out input) == false))
                {
                    Console.WriteLine("You entered an invalid input.");
                    input = 10;
                }

                switch (input)
                {
                    case 1:
                        return ActionType.Create;
                    case 2:
                        return ActionType.Delete;
                    case 3:
                        return ActionType.ReadCart;
                    case 4:
                        return ActionType.Read;
                    case 5:
                        return ActionType.Sort;
                    case 6:
                        return ActionType.Checkout;
                    case 7:
                        return ActionType.Save;
                    case 8:
                        return ActionType.Load;
                    case 9:
                        return ActionType.Exit;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        Console.WriteLine("");
                        continue;

                }
            }
        }

        public static ActionType PrintInventoryMenu()
        {
        

       

            while (true)
            {
                Console.WriteLine("Select an option to begin:");
                Console.WriteLine("1. Add a to the inventory");
                Console.WriteLine("2. List inventory");
                Console.WriteLine("3. Make an item BOGO");
                Console.WriteLine("4. Update a item in inventory");
                Console.WriteLine("5. Delete an item from inventory");
                Console.WriteLine("6. Save inventory");
                Console.WriteLine("7. Load inventory");
                Console.WriteLine("8. Exit inventory");

                var stringInput = (Console.ReadLine() ?? "0");
                int input;
                if ((int.TryParse(stringInput, out input) == false))
                {
                    Console.WriteLine("You entered an invalid input.");
                    input = 10;
                }
                switch (input)
                {
                    case 1:
                        return ActionType.Create;
                    case 2:
                        return ActionType.Read;
                    case 3:
                        return ActionType.BOGO;
                    case 4:
                        return ActionType.Update;
                    case 5:
                        return ActionType.Delete;
                    case 6:
                        return ActionType.Save;
                    case 7:
                        return ActionType.Load;
                    case 8:
                        return ActionType.Exit;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        Console.WriteLine("");
                        continue;

                }
            }
        }
    }

    public enum ActionType
    {
        Create, Read, ReadIncomplete, Update, Delete, Exit, Save, Load, Sort, ReadCart, Checkout, BOGO
    }

    public enum ItemType
    {
        Todo, Appointment
    }
}
