using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.TaskManagement.Models
{
    public class ProductByQuantity : Item
    {
        //should calculate the price using an integer valued property called "Quantity"
        //that keeps track of how many of a given item exists in either the inventory or the cart.
        //public double PricePerUnit { set; get; }
        //public double TotalPrice { set; get; }
        public int Quantity { set; get; }
        


        public ProductByQuantity()
        {
            BOGO = false;
        }

        public override string ToString()
        {
            return $"ID: {Id} :: Name: {Name} :: Description: {Description} :: PricePerUnit: {PricePerUnit} :: Quantity: {Quantity} :: TotalPrice: {TotalPrice} :: BOGO: {BOGO}";
        }

        

        //public DateTime Start { get; set; }
        //public DateTime End { get; set; }
        //public List<string> Attendees { get; set; }
        /*
        public Appointment(string N, string D, double P, int Q, double TP)
        {
            Name = N;
            Description = D;
            Price = P;
            Quantity = Q;
            TotalPrice = TP;
        }
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
            return $"{Name} :: {Description} :: {Price} :: {Quantity} :: {TotalPrice} :: {Completed} :: ";
        }
        */
    }
}
