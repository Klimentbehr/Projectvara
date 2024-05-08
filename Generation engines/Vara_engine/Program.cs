using Main;
using System;
using System.Threading.Tasks;
using Vara_engine_main; 

namespace NPCGenerator
{
    class EntryPoint
    {
        static async Task Main(string[] args)
        {
            bool exit = false;
            while (!exit)
            {
                Console.Clear(); // Clear the console screen before displaying the menu
                Console.WriteLine(" __      __                                                _   _                               _         __      __        ___   ___  \r\n \\ \\    / /                                               | | (_)                             (_)        \\ \\    / /       / _ \\ / _ \\ \r\n  \\ \\  / /_ _ _ __ __ _     __ _  ___ _ __   ___ _ __ __ _| |_ _  ___  _ __    ___ _ __   __ _ _ _ __   __\\ \\  / /__ _ __| | | | | | |\r\n   \\ \\/ / _` | '__/ _` |   / _` |/ _ \\ '_ \\ / _ \\ '__/ _` | __| |/ _ \\| '_ \\  / _ \\ '_ \\ / _` | | '_ \\ / _ \\ \\/ / _ \\ '__| | | | | | |\r\n    \\  / (_| | | | (_| |  | (_| |  __/ | | |  __/ | | (_| | |_| | (_) | | | ||  __/ | | | (_| | | | | |  __/\\  /  __/ |  | |_| | |_| |\r\n     \\/ \\__,_|_|  \\__,_|   \\__, |\\___|_| |_|\\___|_|  \\__,_|\\__|_|\\___/|_| |_| \\___|_| |_|\\__, |_|_| |_|\\___| \\/ \\___|_|   \\___(_)___/ \r\n                     ______ __/ |                                         ______          __/ |          ______     ______            \r\n                    |______|___/                                         |______|        |___/          |______|   |______|           ");
                Console.WriteLine("1. Generate NPCs");
                Console.WriteLine("2. Generate Guns");
                Console.WriteLine("3. Generate Food Items");
                Console.WriteLine("4. Exit");
                Console.Write("Please choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        await Program.RunNPCGenerator();
                        break;
                    case "2":
                        Console.Write("Enter the number of guns to generate: ");
                        if (int.TryParse(Console.ReadLine(), out int numberOfGuns) && numberOfGuns > 0)
                        {
                            GunGeneration gunGeneration = new GunGeneration();
                            await gunGeneration.GenerateAndSaveRandomGuns(numberOfGuns);
                            Console.WriteLine($"{numberOfGuns} guns have been generated and saved.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive integer.");
                        }
                        break;
                    case "3":
                        Console.Write("Enter the path to the food items file: ");
                        string foodItemsFilePath = Console.ReadLine();
                        string[] foodItemsFileLines = File.ReadAllLines(foodItemsFilePath);
                        var foodItems = FoodItemGeneration.GenerateFoodItems(foodItemsFileLines);
                        // Output generated food items
                        foreach (var foodItem in foodItems)
                        {
                            Console.WriteLine($"Name: {foodItem.Name}");
                            Console.WriteLine($"Health Regain: {foodItem.HealthRegain}");
                            Console.WriteLine("Stat Bonuses:");
                            foreach (var statBonus in foodItem.StatBonuses)
                            {
                                Console.WriteLine($"{statBonus.Key}: +{statBonus.Value}");
                            }
                            Console.WriteLine();
                        }
                        break;

                }

                if (!exit)
                {
                    Console.WriteLine(); 
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(); 
                }
            }
        }
    }
}
