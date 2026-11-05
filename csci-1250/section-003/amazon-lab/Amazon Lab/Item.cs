using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Amazon_Lab
{
    internal class Item
    {
        private string name;
        private string description;
        private decimal price;
        private int quantity;

      
        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public decimal Price
        {
            get => price;
            set => price = value;
        }

        public int Quantity
        {
            get => quantity;
            set => quantity = value;
        }

        
        public Item()
        {
            name = "Unnamed Item";
            description = "No description provided";
            price = 0.00m;
            quantity = 0;
        }

        
        public Item(string name, string description, decimal price, int quantity)
        {
            this.name = name;
            this.description = description;
            this.price = price;
            this.quantity = quantity;
        }

       
        public Item(Item other)
        {
            this.name = other.name;
            this.description = other.description;
            this.price = other.price;
            this.quantity = other.quantity;
        }

        
        public bool Equals(Item other)
        {
            return this.name == other.name && this.description == other.description;
        }


        public bool IsOutOfStock()
        {
            return quantity == 0;
        }

        
        public override string ToString()
        {
            if (IsOutOfStock())
                return $"{name} - out of stock";
            else
                return $"{name} - ${price:F2}";
        }

        
        public string GetDetails()
        {
            return $"Item Name: {name}\n" +
                   $"Description: {description}\n" +
                   $"Price: ${price:F2}\n" +
                   $"Quantity Available: {quantity}";
        }

        
        public string GetCSV()
        {
            return $"{name},{description},{price},{quantity}";
        }

        
        public void AddToInventory(int amount)
        {
            if (amount > 0)
                quantity += amount;
        }

       
        public void RemoveFromInventory(int amount)
        {
            if (amount > 0)
                quantity -= amount;

            if (quantity < 0)
                quantity = 0;
        }
    }

}
