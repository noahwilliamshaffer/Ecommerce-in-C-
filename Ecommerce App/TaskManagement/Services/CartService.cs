using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Services
{

    public class CartService
    {
        int perPage;
        private ListNavigator<Item> listNavigator;
        public List<Item> itemList;
        //private List<Item> itemList;

        public List<Item> Items
        {
            get
            {
                return itemList;
            }
        }

        public int NextId
        {
            get
            {
                if (!Items.Any())
                {
                    return 1;
                }

                return Items.Select(t => t.Id).Max() + 1;
            }
        }

        private static CartService current;
        //public ItemService current;

        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }

                return current;
            }
        }

        private CartService()
        //public ItemService()
        {
            itemList = new List<Item>();

            listNavigator = new ListNavigator<Item>(itemList);
        }

        public void Create(Item todo)
        {
            todo.Id = NextId;
            Items.Add(todo);
        }

        public void Update(Item? todo)
        {

        }

        public void Delete(int id)
        {
            var todoToDelete = itemList.FirstOrDefault(t => t.Id == id);
            if (todoToDelete == null)
            {
                return;
            }
            itemList.Remove(todoToDelete);
            listNavigator = new ListNavigator<Item>(itemList);

        }

        public void AddProductByWeight(int productNumber, int numAdded, ItemService inventory)
        {
            if (inventory.itemList[productNumber] is ProductByWeight)
            {
                if (inventory.itemList[productNumber] is ProductByWeight)
                {
                    if (numAdded > ((ProductByWeight)inventory.itemList[productNumber]).Pounds)
                    {
                        Console.WriteLine("We don't have enough weight of that item in stock.");
                        return;
                    }
                    //item selected is already in cart
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (itemList[i].Name == inventory.itemList[productNumber].Name)
                        {
                            Console.WriteLine("You already have that item in your cart");
                            ((ProductByWeight)inventory.Items[productNumber]).Pounds = ((ProductByWeight)inventory.Items[productNumber]).Pounds - numAdded;
                            ((ProductByWeight)Items[productNumber]).Pounds = ((ProductByWeight)Items[productNumber]).Pounds + numAdded;

                            //((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerPound;
                            //((ProductByWeight)Items[productNumber]).TotalPrice = ((ProductByWeight)Items[productNumber]).PricePerPound * ((ProductByWeight)Items[productNumber]).Pounds;
                            ((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerUnit;
                            ((ProductByWeight)Items[productNumber]).TotalPrice = ((ProductByWeight)Items[productNumber]).PricePerUnit * ((ProductByWeight)Items[productNumber]).Pounds;
                            return;
                        }

                    }
                    var prodQ = inventory.Items[productNumber] as ProductByWeight;
                    ProductByWeight cartProduct = new ProductByWeight();
                    cartProduct.Id = prodQ.Id;
                    cartProduct.Name = prodQ.Name;
                    cartProduct.Description = prodQ.Description;
                    //cartProduct.PricePerPound = prodQ.PricePerPound;
                    cartProduct.PricePerUnit = prodQ.PricePerUnit;
                    cartProduct.Pounds = numAdded;
                    ((ProductByWeight)inventory.Items[productNumber]).Pounds -= numAdded;

                    //((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerPound;
                    ((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerUnit;
                    //cartProduct.TotalPrice = cartProduct.PricePerPound * cartProduct.Pounds;

                    cartProduct.TotalPrice = cartProduct.PricePerUnit * cartProduct.Pounds;
                    Items.Add(cartProduct);
                }

            }
            listNavigator = new ListNavigator<Item>(itemList);

        }

        public void Create(int productNumber, int numAdded, ItemService inventory)
        {
            if (inventory.itemList[productNumber] is ProductByQuantity)
            {
                AddProductByQuantity(productNumber, numAdded, inventory);
            }
            else
            {
                AddProductByWeight(productNumber, numAdded, inventory);
            }
            listNavigator = new ListNavigator<Item>(itemList);
        }
        public void AddProductByQuantity(int productNumber, int numAdded, ItemService inventory)
        {
            if (inventory.itemList[productNumber] is ProductByQuantity)
            {
                if (inventory.itemList[productNumber] is ProductByQuantity)
                {
                    if (numAdded > ((ProductByQuantity)inventory.itemList[productNumber]).Quantity)
                    {
                        Console.WriteLine("We don't have enough quantity of that item in stock.");
                        return;
                    }
                    //item selected is already in cart
                    for (int i = 0; i < itemList.Count; i++)
                    {
                        if (itemList[i].Name == inventory.itemList[productNumber].Name)
                        {
                            Console.WriteLine("You already have that item in your cart");
                            ((ProductByQuantity)inventory.Items[productNumber]).Quantity = ((ProductByQuantity)inventory.Items[productNumber]).Quantity - numAdded;
                            ((ProductByQuantity)Items[productNumber]).Quantity = ((ProductByQuantity)Items[productNumber]).Quantity + numAdded;

                            ((ProductByQuantity)inventory.Items[productNumber]).TotalPrice = ((ProductByQuantity)inventory.Items[productNumber]).Quantity * ((ProductByQuantity)inventory.Items[productNumber]).PricePerUnit;
                            ((ProductByQuantity)Items[productNumber]).TotalPrice = ((ProductByQuantity)Items[productNumber]).PricePerUnit * ((ProductByQuantity)Items[productNumber]).Quantity;
                            return;
                        }

                    }
                    var prodQ = inventory.Items[productNumber] as ProductByQuantity;
                    ProductByQuantity cartProduct = new ProductByQuantity();
                    cartProduct.Id = prodQ.Id;
                    cartProduct.Name = prodQ.Name;
                    cartProduct.Description = prodQ.Description;
                    cartProduct.PricePerUnit = prodQ.PricePerUnit;
                    cartProduct.Quantity = numAdded;
                    ((ProductByQuantity)inventory.Items[productNumber]).Quantity -= numAdded;

                    ((ProductByQuantity)inventory.Items[productNumber]).TotalPrice = ((ProductByQuantity)inventory.Items[productNumber]).Quantity * ((ProductByQuantity)inventory.Items[productNumber]).PricePerUnit;
                    //if bogo
                    //devide by
                    cartProduct.BOGO = inventory.Items[productNumber].BOGO;
                    if (inventory.Items[productNumber].BOGO == true)
                    {
                        int bogo = cartProduct.Quantity / 2;
                        bogo = bogo +cartProduct.Quantity % 2;
                        cartProduct.TotalPrice = cartProduct.PricePerUnit * bogo;
                    }
                    else
                    {
                        cartProduct.TotalPrice = cartProduct.PricePerUnit * cartProduct.Quantity;
                    }
                    
                    Items.Add(cartProduct);
                }

            }
            listNavigator = new ListNavigator<Item>(itemList);
        }
        /*
        public void Create(int productNumber, int numAdded, ItemService inventory)
        {
            //Too many items selected
            
            if(numAdded > inventory.itemList[productNumber].Quantity)
            {
                Console.WriteLine("We don't have that many items in stock.");
                return;
            }
            //item selected is already in cart
           for(int i= 0; i < itemList.Count; i++)
            {
                if(itemList[i].Name == inventory.itemList[productNumber].Name)
                {
                    Console.WriteLine("You already have that item in your cart");
                    inventory.Items[productNumber].Quantity = inventory.Items[productNumber].Quantity - numAdded;
                    Items[productNumber].Quantity = Items[productNumber].Quantity + numAdded;

                    inventory.Items[productNumber].TotalPrice = inventory.Items[productNumber].Quantity * inventory.Items[productNumber].Price;
                    Items[productNumber].TotalPrice = Items[productNumber].Price * Items[productNumber].Quantity;
                    return;
                }
    
            }

            //if the item is not present in the cart
            if (inventory.Items[productNumber] is ProductByWeight)
            {
                var prodW = inventory.Items[productNumber] as ProductByWeight;
                ProductByWeight cartProduct = new ProductByWeight();
                cartProduct.Id = prodW.Id;
                cartProduct.Name = prodW.Name;
                cartProduct.Description = prodW.Description;
                cartProduct.PricePerPound = prodW.PricePerPound;
                cartProduct.Pounds = prodW.Pounds;
                cartProduct.TotalPrice = prodW.TotalPrice;
                cartProduct.Pounds = prodW.Pounds;
                ((ProductByWeight)inventory.Items[productNumber]).Pounds = ((ProductByWeight)inventory.Items[productNumber]).Pounds - numAdded;

                inventory.Items[productNumber].TotalPrice = inventory.Items[productNumber].Quantity * inventory.Items[productNumber].Price;
                cartProduct.TotalPrice = cartProduct.Price * cartProduct.Quantity;
                Items.Add(cartProduct);

            }
            else
            {
                var prodQ = inventory.Items[productNumber] as ProductByQuantity;
                ProductByQuantity cartProduct = new ProductByQuantity();
                cartProduct.Id = prodQ.Id;
                cartProduct.Name = prodQ.Name;
                cartProduct.Description = prodQ.Description;
                cartProduct.Price = prodQ.Price;
                cartProduct.PriceQuantity = prodQ.PriceQuantity;
                cartProduct.Quantity = numAdded;
                inventory.Items[productNumber].Quantity = inventory.Items[productNumber].Quantity - numAdded;

                inventory.Items[productNumber].TotalPrice = inventory.Items[productNumber].Quantity * inventory.Items[productNumber].Price;
                cartProduct.TotalPrice = cartProduct.Price * cartProduct.Quantity;
                Items.Add(cartProduct);
            }

            
            
        */


        public void Delete(int productNumber, int numRemoved, ItemService inventory)
        {
            if (itemList[productNumber] is ProductByQuantity)
            {
                DeleteProductByQuantity(productNumber, numRemoved, inventory);
            }
            else
            {
                DeleteProductByWeight(productNumber, numRemoved, inventory);
            }
            listNavigator = new ListNavigator<Item>(itemList);
        }

        public void DeleteProductByWeight(int productNumber, int numRemoved, ItemService inventory)
        {
            /*
            var todoToDelete = itemList.FirstOrDefault(t => t.Id == id);
            if (todoToDelete == null)
            {
                Console.WriteLine("No product associated with that product number.");
                return;
            }

            */
            if (numRemoved >= ((ProductByWeight)Items[productNumber]).Pounds)
            {
                ((ProductByWeight)inventory.Items[productNumber]).Pounds = ((ProductByWeight)inventory.Items[productNumber]).Pounds + ((ProductByWeight)Items[productNumber]).Pounds;
                Delete(itemList[productNumber].Id); //Items.Remove(Items[productNumber]);
                return;
            }
            Console.WriteLine($"Removing products");
            ((ProductByWeight)Items[productNumber]).Pounds = ((ProductByWeight)Items[productNumber]).Pounds - numRemoved;
            ((ProductByWeight)inventory.Items[productNumber]).Pounds = ((ProductByWeight)inventory.Items[productNumber]).Pounds + numRemoved;

            //((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerPound;
            //((ProductByWeight)Items[productNumber]).TotalPrice = ((ProductByWeight)Items[productNumber]).PricePerPound * ((ProductByWeight)Items[productNumber]).Pounds;
            ((ProductByWeight)Items[productNumber]).TotalPrice = ((ProductByWeight)Items[productNumber]).PricePerUnit * ((ProductByWeight)Items[productNumber]).Pounds;
            ((ProductByWeight)inventory.Items[productNumber]).TotalPrice = ((ProductByWeight)inventory.Items[productNumber]).Pounds * ((ProductByWeight)inventory.Items[productNumber]).PricePerUnit;

            listNavigator = new ListNavigator<Item>(itemList);
        }


        public void DeleteProductByQuantity(int productNumber, int numRemoved, ItemService inventory)
        {
            /*int productNumber = 0;
            for (int i = 0; i < inventory.itemList.Count(); i++)
            {
                if (id == inventory.itemList[i].Id)
                {
                    productNumber = i;
                }
            }
            */
            /*
            var todoToDelete = itemList.FirstOrDefault(t => t.Id == productNumber);
            if (todoToDelete == null)
            {
                Console.WriteLine("No product associated with that product number.");
                return;
            }
            */
            if (numRemoved >= ((ProductByQuantity)Items[productNumber]).Quantity)
            {
                ((ProductByQuantity)inventory.Items[productNumber]).Quantity = ((ProductByQuantity)inventory.Items[productNumber]).Quantity + ((ProductByQuantity)Items[productNumber]).Quantity;
                Delete(itemList[productNumber].Id); //Items.Remove(Items[productNumber]);
                return;
            }
            Console.WriteLine($"Removing products");
            ((ProductByQuantity)Items[productNumber]).Quantity = ((ProductByQuantity)Items[productNumber]).Quantity - numRemoved;
            ((ProductByQuantity)inventory.Items[productNumber]).Quantity = ((ProductByQuantity)inventory.Items[productNumber]).Quantity + numRemoved;

            ((ProductByQuantity)inventory.Items[productNumber]).TotalPrice = ((ProductByQuantity)inventory.Items[productNumber]).Quantity * ((ProductByQuantity)inventory.Items[productNumber]).PricePerUnit;
            ((ProductByQuantity)Items[productNumber]).TotalPrice = ((ProductByQuantity)Items[productNumber]).PricePerUnit * ((ProductByQuantity)Items[productNumber]).Quantity;
            listNavigator = new ListNavigator<Item>(itemList);
        }
        /*
        public void Delete(int productNumber, int numRemoved, ItemService inventory)
        {
            
            var todoToDelete = itemList.FirstOrDefault(t => t.Id == productNumber);
            if (todoToDelete == null)
            {
                Console.WriteLine("No product associated with that product number.");
                return;
            }
            
        

            if(numRemoved >= Items[productNumber].Quantity)
            {
                inventory.Items[productNumber].Quantity = inventory.Items[productNumber].Quantity + Items[productNumber].Quantity;
                Items.Remove(Items[productNumber]);
                return;
            }
            Console.WriteLine($"Removing products");
            Items[productNumber].Quantity = Items[productNumber].Quantity - numRemoved;
            inventory.Items[productNumber].Quantity = inventory.Items[productNumber].Quantity + numRemoved;

            inventory.Items[productNumber].TotalPrice = inventory.Items[productNumber].Quantity * inventory.Items[productNumber].Price;
            Items[productNumber].TotalPrice = Items[productNumber].Price * Items[productNumber].Quantity;
        }
*/
       /*

        public void Sort()
        {
            listNavigator = new ListNavigator<Item>(itemList);

            Console.WriteLine("2. Sort by ");
            Console.WriteLine("3. Sort by unit price inventory(inventory)");
            
            if (sortMethod == 1)
            {
                Console.WriteLine("Enter the name you would like to search for.");
                var name = Console.ReadLine() ?? "0";

                Search(name);
            }
            else if (sortMethod == 2)
            {
                Console.WriteLine("You chose to sort by total price. (cart)");
                listNavigator = new ListNavigator<Item>(itemList);
            }
            else if (sortMethod == 3)
            {
                Console.WriteLine("You chose to sort by unit price inventory(inventory)");

            }
            else
            {
                Console.WriteLine("No changes made.)");
            }
            listNavigator = new ListNavigator<Item>(itemList);
        }
       */
        public void Load(string fileName)
        {
            var todosJson = File.ReadAllText(fileName);
            itemList = JsonConvert.DeserializeObject<List<Item>>
                (todosJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Item>();

        }

        public void Save(string fileName)
        {
            var todosJson = JsonConvert.SerializeObject(itemList
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, todosJson);
        }


        
        /*
        public void Display()
        { 
            bool run = true;
            int perPage = 5;
           
            Console.WriteLine("How many items do you want per page?");
            var pages = Console.ReadLine();
            if(int.TryParse(pages, out perPage) == false)
            {
                perPage = 5;
            }
            listNavigator = new ListNavigator<Item>(itemList,perPage);



            while (run)
            {
               
                    for (int i = 0; i < perPage; i++)
                    {
                    if (itemList.Count >= i)
                    {
                        var InventoryProduct = Items[i];
                        Console.WriteLine($"{InventoryProduct}");
                    }
                    }

                    Console.WriteLine("1. Next Page");
                    Console.WriteLine("2. Previous Page");
                    Console.WriteLine("3. Change number of items per page.");
                    Console.WriteLine("3. Exit");

            }
                listNavigator.GetCurrentPage();
                Console.WriteLine("Running pages...");
               
                var pageOption = Console.ReadLine();


            
        }
        */
       




        public void Display()
        {
            bool Nextpage = false;
            bool PreviousPage = false; ;
            int maxPage = 1;
            bool run = true;
            int perPage = 5;
            int Page = 1;

            //int itemsPrev = 0;
            /*
            Console.WriteLine("How many items do you want per page?");
            var pages = Console.ReadLine() ?? "5";
            if (int.TryParse(pages, out perPage) == false)
            {
                perPage = 5;
            }
            */
            listNavigator = new ListNavigator<Item>(itemList, perPage);



            while (run)
            {
                maxPage = itemList.Count / perPage;
                if(maxPage == 0)
                {
                    maxPage = 1;
                }
                if (maxPage % itemList.Count > 0)
                {
                    maxPage++;
                }
                var itemsPrev = (Page - 1) * perPage;

                Console.WriteLine($"Page: {Page} of {maxPage}");

                Console.WriteLine("");
                //for (int i = 0; i < perPage; i++)
                //{

                for (int j = itemsPrev; j < perPage + itemsPrev; j++)
                {
                    if (j < itemList.Count)
                    {
                        var InventoryProduct = Items[j];
                        Console.WriteLine($"{InventoryProduct}");
                    }
                }
                //}

                Console.WriteLine("1. Next Page");
                Console.WriteLine("2. Previous Page");
                Console.WriteLine("3. Change number of items per page.");
                Console.WriteLine("4. Exit");
                Console.WriteLine("");
                var Option = Console.ReadLine() ?? "5";
                if (Option == "1")
                {
                    Console.WriteLine("32");
                    if (Page < maxPage)
                    {
                        Page++;
                    }

                }
                else if (Option == "2")
                {
                    if (Page > 1)
                    {
                        Page--;
                    }
                }
                else if (Option == "3")
                {
                    Console.WriteLine("How many items perPage do you want to display?");
                    string perPagestr = Console.ReadLine() ?? "5";
                    if (int.TryParse(perPagestr, out perPage) == false)
                    {
                        perPage = 5;
                    }

                }
                else if (Option == "4")
                {
                    Console.WriteLine("Exiting.");
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid input entered.");
                }


            }
        }
        
        public void Checkout()
        {
            double totalAtCheckout = 0;

            
            for (int i = 0; i < itemList.Count; i++)
            {
                if(itemList[i] is ProductByWeight)
                    totalAtCheckout += ((ProductByWeight)itemList[i]).TotalPrice * 1.07;
                if (itemList[i] is ProductByQuantity)
                    totalAtCheckout += ((ProductByQuantity)itemList[i]).TotalPrice * 1.07;
            }


            Console.WriteLine($"The total at checkout is currently after tax: {totalAtCheckout,2} $");
            Console.WriteLine("Would you like to proceeds with your order?:");
            Console.WriteLine("Yes [1]");
            Console.WriteLine("no [2]");

            
            var Order = Console.ReadLine();
            string str;

            if (Order == "1")
            {
                Console.WriteLine("Would you like to checkout with card or bitcoin?:");
                Console.WriteLine("Card [1]");
                Console.WriteLine("Bitcoin [2]");
                string payMethod = Console.ReadLine() ?? "5";
                if(payMethod == "1")
                {
                    Console.Write("Enter your credit card number:");
                    str = Console.ReadLine() ?? "5";
                    Console.Write("Enter your credit card expiration date:");
                    str = Console.ReadLine() ?? "5";
                    Console.Write("Enter your credit card number CCV:");
                    str = Console.ReadLine() ?? "5";
                    Console.Write("Thank you for your payment.");
                }
                else if (payMethod == "2")
                {
                    Console.Write("Enter a valid bitcoin address.");
                    str = Console.ReadLine() ?? "5";
                    Console.Write("Thank you for your payment.");
                }
                else
                {
                    Console.WriteLine("Error at checkout!");
                }
                itemList.Clear();
                Console.WriteLine("You cart has been cleared and your checkout is complete have a nice day!");
                Environment.Exit(0);
            }
            else if (Order == "2")
            {
                return;
            }
            else
            {
                Console.WriteLine("Error at checkout!");
                return;
            }
        }

            public void SortByTotalPrice()//cart
            {
            //change pricepPerunit across all files keep variab
            var orderedListEnum = itemList.OfType<Item>();
            var orderedList = from Item in itemList orderby Item.PricePerUnit select Item;
            foreach(var item in orderedList)
            {
                var InventoryProduct = item;
                Console.WriteLine($"{InventoryProduct}");
            }
        }
            
    }

}