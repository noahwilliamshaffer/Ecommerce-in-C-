using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//This becomes product class
namespace Library.TaskManagement.Interfaces
{
    internal interface IItem
    {
        //public string NA { set; get; }
       // public string DE { set; get; }
       // public double PR { set; get; }// - the unit price of the product


        /*
         public string Name { set; get; }// - the name of the product
         public string Description { set; get; }// - the description of the product
         public double Price { set; get; }// - the unit price of the product
         public int Quantity { set; get; }// - the number of units being purchased
         public double TotalPrice { set; get; }//- the total price of the product being purchased(i.e., Price* Quantity)
         public bool Completed { set; get; }
         */
        //INotifyPropertyChanged

        public double GreaterThan()
        {
            return 0;
        }
    }
}
