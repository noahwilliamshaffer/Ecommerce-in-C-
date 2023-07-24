using Library.TaskManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public partial class ProductByWeight : Item
    {
        //should calculate the price using a real valued property called "Weight" that measures weight to the tenth
        //(it is ok to use more precision but not less).
        //they are buying the quantity in pounds
        //empoyee can enter 1 onz 10 pounds
        //this could accept a double 

        public double Pounds { set; get; }
        //public double PricePerPound{set; get; }
        //public double TotalPrice { set; get; }
        

        public ProductByWeight()
        {
            BOGO = false;
        }

        
        public ProductByWeight(string N, string D, double P, int Q, double TP)
        {
            Name = N;
            Description = D;
        }

        /*
        public void Print()
        {
            Console.WriteLine($"Name : {Name}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Price: {Price} $");
            Console.WriteLine($"Quantity: {Quantity}");
            Console.WriteLine($"TotalPrice: {TotalPrice} $");
        }
        

        public override string ToString()
        {
            return $"{Id} {Deadline:d} - {Name} :: {Description}";
        }
        */
        
        public override string ToString()
        {
            return $"ID: {Id} :: Name: {Name} :: Description: {Description} :: PricePerPound: {PricePerUnit} :: Pounds: {Pounds} :: TotalPrice: {TotalPrice}";
        }
    }
}
