/**
*--------------------------------------------------------------------
*File name: Audiobooks.cs
* Project name: Audiobooks
* Solution name: Audiobooks
* --------------------------------------------------------------------
*Author’s name and email: Daniel [REDACTED EMAIL]
* Course-Section: CSCI 1250
* Creation Date: Sept. 20, 2023
* Modified Date: Sept. 20, 2023
* -------------------------------------------------------------------
*/
namespace Assignment_03___Decision_Structures
{
    /// <summary>
    ///
    /// </summary>
    internal class Program
    {

        /// <summary>
        ///
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            const double PackageAPrice = 9.95;
            const double PackageBPrice = 13.95;
            const double PackageCPrice = 19.95;
            const double AdditionalBookPriceA = 2.00;
            const double AdditionalBookPriceB = 1.00;
            const int BooksIncludedA = 10;
            const int BooksIncludedB = 20;
            char choice;
            int numBooks;
            double totalCost = 0.0;
            totalCost = 0;

            Console.Write("Enter the letter of the Audiobooks package you purchased (A, B, or C): ");
            choice = Convert.ToChar(Console.ReadLine());


            Console.Write("Enter the number of audiobooks that you read: ");
            numBooks = int.Parse(s: Console.ReadLine());



            if (choice == 'a')
            {

                totalCost = PackageAPrice;
                if (numBooks > 10)
                {
                    numBooks = numBooks - 10;
                    totalCost = PackageAPrice + (numBooks * 2);

                }

            }
            else if (choice == 'b')

                totalCost = PackageBPrice;
            if (numBooks > 20)
            {
                numBooks = numBooks - 20;
                totalCost = PackageBPrice + (numBooks * 1);

            }

            else if (choice == 'c')
            {
                totalCost = PackageCPrice;
            }
            else
            {
                Console.WriteLine("Wrong input. Please enter A, B, or C.");
                return;
            }


            Console.WriteLine("Total charges: $" + totalCost.ToString("F2"));
        }
        
    }

}


