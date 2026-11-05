namespace Amazon_Lab
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Inventory inventory = new Inventory();
            bool exit = false;

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("========================================");
                Console.WriteLine("Welcome to Yangtze River Online Sales Company");
                Console.WriteLine("========================================");
                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. View Product Inventory");
                Console.WriteLine("2. View Shopping Cart");
                Console.WriteLine("3. Checkout");
                Console.WriteLine("5. Add Product to Inventory");
                Console.WriteLine("6. Remove Product from Inventory");
                Console.WriteLine("7. Save Data");
                Console.WriteLine("8. Load Data");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your selection (1-9): ");

                string choice = Console.ReadLine();
                Console.Clear();

                if (choice == "1")
                {
                    Console.WriteLine("Product Inventory:");
                    inventory.DisplayInventory();
                    Pause();
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Shopping cart is currently under construction.");
                    Pause();
                }
                else if (choice == "3")
                {
                    Console.WriteLine("Checkout is currently under construction.");
                    Pause();
                }
                else if (choice == "5")
                {
                    Console.Write("Enter product name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter description: ");
                    string description = Console.ReadLine();

                    Console.Write("Enter price: ");
                    decimal price = decimal.Parse(Console.ReadLine());

                    Console.Write("Enter quantity: ");
                    int quantity = int.Parse(Console.ReadLine());

                    Item newItem = new Item(name, description, price, quantity);
                    inventory.AddItem(newItem);

                    Console.WriteLine("Product added!");
                    Pause();
                }
                else if (choice == "6")
                {
                    inventory.DisplayInventory();
                    Console.Write("Enter the index of the product to remove: ");
                    int removeIndex = int.Parse(Console.ReadLine());

                    if (removeIndex >= 0 && removeIndex < inventory.GetStuffForSale().Count)
                    {
                        Item itemToRemove = inventory.GetStuffForSale()[removeIndex];
                        inventory.RemoveItem(itemToRemove);
                        Console.WriteLine("Product removed.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid index.");
                    }

                    Pause();
                }
                else if (choice == "7")
                {
                    Console.Write("Enter filename to save to: ");
                    string saveFile = Console.ReadLine();
                    inventory.Save(saveFile);
                    Pause();
                }
                else if (choice == "8")
                {
                    Console.Write("Enter filename to load from: ");
                    string loadFile = Console.ReadLine();
                    inventory.Load(loadFile);
                    Pause();
                }
                else if (choice == "9")
                {
                    exit = true;
                    Console.WriteLine("Exiting the program...");
                }
                else
                {
                    Console.WriteLine("Invalid selection. Try again.");
                    Pause();
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nPress Enter to continue...");
            Console.ReadLine();
        }
    }
}
