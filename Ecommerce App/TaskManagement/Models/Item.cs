using Library.TaskManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class Item
    {
        //put price per unit 
        public double TotalPrice { set; get; }
        public int Id { get; set; }
        public string Name { set; get; }// - the name of the product
        public string Description { set; get; }// - the description of the product
        public double PricePerUnit { set; get; }
        public bool BOGO { set; get; }

        //public double Price { set; get; }// - the unit price of the product
        //public int Quantity { set; get; }// - the number of units being purchased
        //public double TotalPrice { set; get; }//- the total price of the product being purchased(i.e., Price* Quantity)
        //public double Pounds { set; get; }//- the total price of the product being purchased(i.e., Price* Quantity)
        //public bool Completed { set; get; }


        public override string ToString()
        {
            return $"{Name} :: {Description} ";
        }

    }
}
