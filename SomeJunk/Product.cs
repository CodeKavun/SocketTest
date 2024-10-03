using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeJunk
{
    public class Product
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Productor { get; set; }

        public Product(string name, double price, string productor)
        {
            Name = name;
            Price = price;
            Productor = productor;
        }

        public override string ToString()
        {
            return $"{Name} ({Productor}) - {Price}UAH";
        }
    }
}
