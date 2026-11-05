using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amazon_Lab
{
    internal class Inventory
    {

        private List<Item> stuffForSale;

        public Inventory()
        {
            stuffForSale = new List<Item>();
        }

        public void DisplayInventory()
        {
            for (int i = 0; i < stuffForSale.Count; i++)
            {
                Console.WriteLine($"{i}: {stuffForSale[i]}");
            }
        }

        public void DisplayDetails(int index)
        {
            if (index >= 0 && index < stuffForSale.Count)
            {
                Console.WriteLine(stuffForSale[index].GetDetails());
            }
            else
            {
                Console.WriteLine("Invalid item index.");
            }
        }

        public void AddInventory(int index, int amount)
        {
            if (index >= 0 && index < stuffForSale.Count)
            {
                stuffForSale[index].AddToInventory(amount);
            }
        }

        public void RemoveInventory(int index, int amount)
        {
            if (index >= 0 && index < stuffForSale.Count)
            {
                stuffForSale[index].RemoveFromInventory(amount);
            }
        }

        public void AddItem(Item thing)
        {
            stuffForSale.Add(thing);
        }

        public void RemoveItem(Item thing)
        {
            stuffForSale.Remove(thing);
        }

        public void PurchaseItem(Item thing)
        {
            for (int i = 0; i < stuffForSale.Count; i++)
            {
                if (stuffForSale[i].Equals(thing))
                {
                    stuffForSale[i].RemoveFromInventory(thing.Quantity);
                    return;
                }
            }
        }

        public List<Item> GetStuffForSale()
        {
            return stuffForSale;
        }

        public void Save(string filePath)
        {
            List<string> lines = new List<string>();
            foreach (Item item in stuffForSale)
            {
                lines.Add(item.GetCSV());
            }
            File.WriteAllLines(filePath, lines.ToArray());
            Console.WriteLine($"Inventory saved to {filePath}");
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            string[] lines = File.ReadAllLines(filePath);
            List<Item> newList = new List<Item>();

            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    string name = parts[0];
                    string description = parts[1];
                    decimal price = decimal.Parse(parts[2]);
                    int quantity = int.Parse(parts[3]);
                    newList.Add(new Item(name, description, price, quantity));
                }
            }

            stuffForSale = newList;
        }
    }
}
