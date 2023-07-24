using Library.TaskManagement.Models;
using Library.TaskManagement.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//look up sort by integer 
namespace Library.TaskManagement.Services
{

    public class ItemService
    {
        int perPage;
        private ListNavigator<Item> listNavigator;
        //private List<Item> itemList; change back later
        public List<Item> itemList;
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

        private static ItemService current;
        //public ItemService current;

        public static ItemService Current
        {
            get
            {
                if (current == null)
                {
                    current = new ItemService();
                }

                return current;
            }
        }

        private ItemService()
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

        public void makeBogo(Item? ItemToUpdate)
        {
            if (ItemToUpdate is ProductByWeight)
            {
                Console.WriteLine("You can't make a product by weight bogo foo!");
            }
            else
            {
                ItemToUpdate.BOGO = true;
            }
        }

        public void Update(Item? ItemToUpdate)
        {
            Console.WriteLine("What would you like to update?");
           

            if (ItemToUpdate is ProductByWeight)
            {
                Console.WriteLine("1. Name");
                Console.WriteLine("2. Description");
                Console.WriteLine("3. Pounds");
                Console.WriteLine("4. PricePerPound");

                var input = Console.ReadLine();

                if(input == "1")
                {
                    string name;
                    Console.Write("Enter a new name: ");
                    name = Console.ReadLine() ?? string.Empty;
                    ItemToUpdate.Name = name;

                }
                else if (input == "2")
                {
                    Console.Write("Enter a new Description: ");
                    var description = Console.ReadLine() ?? string.Empty;
                    //description = Console.ReadLine() ?? string.Empty;
                    ItemToUpdate.Description = description;

                }
                else if (input == "3")
                {
                    Console.Write("Enter a new weight in Pounds: ");
                    string v = Console.ReadLine() ?? string.Empty;
                    if (!(double.TryParse(v, out double pounds)))
                    {
                        Console.Write("Invalid number entered");
                        return;
                    }

                    ((ProductByWeight)ItemToUpdate).Pounds = pounds;

                }
                else if (input == "4")
                {
                    Console.Write("Enter a new price per pound. ");
                    string v = Console.ReadLine() ?? string.Empty;
                    if (!(double.TryParse(v, out double priceperpounds)))
                    {
                        Console.Write("Invalid number entered");
                        return;
                    }

                    ((ProductByWeight)ItemToUpdate).Pounds = priceperpounds;

                }
                else
                {
                    Console.WriteLine("invalid number selected");
                }




            }
            if (ItemToUpdate is ProductByQuantity)
            {
                
                

                Console.WriteLine("1. Name");
                Console.WriteLine("2. Description");
                Console.WriteLine("3. Quantity");
                Console.WriteLine("4. PricePerUnit");
                var input = Console.ReadLine() ?? string.Empty;

                if (input == "1")
                {
                    string name;
                    Console.Write("Enter a new name: ");
                    name = Console.ReadLine() ?? string.Empty;
                    ItemToUpdate.Name = name;
                }
                else if (input == "2")
                {
                    Console.Write("Enter a new Description: ");
                    var description = Console.ReadLine() ?? string.Empty;
                    //description = Console.ReadLine() ?? string.Empty;
                    ItemToUpdate.Description = description;
                }
                else if (input == "3")
                {
                    Console.Write("Enter a new quantity: ");
                    string v = Console.ReadLine() ?? string.Empty;
                    if(!(Int32.TryParse(v, out int quantity)))
                    {
                        Console.Write("Invalid number entered");
                        return;
                    }

                    ((ProductByQuantity)ItemToUpdate).Quantity = quantity;

                }
                else if (input == "4")
                {
                    Console.Write("Enter a new price per unit: ");
                    string p = Console.ReadLine() ?? string.Empty;
                    if (!(double.TryParse(p, out double price)))
                    {
                        Console.Write("Invalid number entered");
                        return;
                    }

                    //(ProductByQuantity)ItemToUpdate).PricePerUnit = price;
                    ItemToUpdate.PricePerUnit = price;

                }
                else
                {
                    Console.WriteLine("invalid choice selected");
                }

            }
            listNavigator = new ListNavigator<Item>(itemList);
        }

        public void Delete(int id)
        {
            var todoToDelete = itemList.FirstOrDefault(t => t.Id == id);
            if (todoToDelete == null)
            {
                return;
            }
            itemList.Remove(todoToDelete);
            
        }
      
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

        //GROSS
        //public IEnumerable<Item> Search(string query)
        //{
        //    return Items.Where(i => i.Description.Contains(query) || i.Name.Contains(query));
        //}

        //Stateful Implementation
        
        
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
                
                maxPage = itemList.Count /perPage;
                var itemsPrev = (Page - 1) * perPage;
                if (maxPage == 0)
                {
                    maxPage = 1;
                }
                if(maxPage % itemList.Count > 0)
                {
                    maxPage++;
                }
                Console.WriteLine($"Page: {Page} of {maxPage}");

                Console.WriteLine($"");
                //for (int i = 0; i < perPage; i++)
                //{
                    
                    for (int j = itemsPrev; j < perPage+itemsPrev; j++)
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
                var Option = Console.ReadLine() ?? "5";
                if (Option == "1")
                {
                    
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
                else if(Option == "3")
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
        public void Sort(CartService cart)
        {
            //as long as you havnt called the new keyword or the to list it works
            _sort = true;

            Console.WriteLine("You chose to sort the cart.");
            Console.WriteLine("What method would you like to sort by?");
            Console.WriteLine("1. Sort by name.");
            Console.WriteLine("2. Sort by total price. (cart)"); //
            Console.WriteLine("3. Sort by unit price inventory(inventory)"); //you list the price in order of whatever price is put in there
            var choice = Console.ReadLine() ?? "0";

            if (choice == "1")
            {
                Console.WriteLine("Enter the name you would like to search for.");
                var name = Console.ReadLine() ?? "0";

                 SortByNameDescription(name);
                /*
                for(int i = 0; i < sortedList.Count(); i++)
                {
                    var sortedItem = Items[i];
                    Console.WriteLine($"{sortedItem}");
                }
                */

            }
            else if (choice == "2")
            {
                Console.WriteLine("You chose to sort the cart by total price");
                cart.SortByTotalPrice();
            }
            else if (choice == "3")
            {

                SortByUnitPrice();
            }
            else
            {
                Console.WriteLine("You chose an invalid option.");
            }
        }


        private string _query;
        private bool _sort;

        public IEnumerable<Item> Search(string query)
        {
            _query = query;
            return ProcessedList;
        }

        
        public IEnumerable<Item> ProcessedList
        {
            get
            {
                if (string.IsNullOrEmpty(_query))
                {
                    return Items;
                }
                //i => string.IsNullOrEmpty(_query) ||
                Console.WriteLine($"Searching for {_query}");
                return Items
                    .Where((i => i.Description.Contains(_query)
                        || i.Name.Contains(_query))) //search
                    .OrderBy(i => i.Name);          //sort

            }
        }
        
        public void SortByNameDescription(string name)
        {
            for (int i = 0; i < itemList.Count(); i++)
            {
                if(Items[i].Name.Contains(name) || Items[i].Description.Contains(name))
                {
                    var sortedItem = Items[i];
                    Console.WriteLine($"{sortedItem}");
                }
                
            }
        }

        public void sortByName()
        {

        }
        public void SortByUnitPrice()//inventory
        {
            var orderedListEnum = itemList.OfType<Item>();
            var orderedList = from Item in itemList orderby Item.PricePerUnit select Item;
            foreach (var item in orderedList)
            {
                var InventoryProduct = item;
                Console.WriteLine($"{InventoryProduct}");
            }

        }

    }

    }

   