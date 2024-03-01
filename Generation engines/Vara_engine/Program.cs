using Main;
using System;
using System.Threading.Tasks; // Ensure this is included for async Task support

namespace NPCGenerator
{
    class EntryPoint
    {
        static async Task Main(string[] args) // Ensure Main is async to allow await
        {
            // Increase the console window size
            //Console.SetWindowSize(Console.WindowWidth * 2, Console.WindowHeight * 2);

            bool exit = false;
            while (!exit)
            {
                Console.Clear(); // Clear the console screen before displaying the menu
                Console.WriteLine(" __      __                                                _   _                               _         __      __        ___   ___  \r\n \\ \\    / /                                               | | (_)                             (_)        \\ \\    / /       / _ \\ / _ \\ \r\n  \\ \\  / /_ _ _ __ __ _     __ _  ___ _ __   ___ _ __ __ _| |_ _  ___  _ __    ___ _ __   __ _ _ _ __   __\\ \\  / /__ _ __| | | | | | |\r\n   \\ \\/ / _` | '__/ _` |   / _` |/ _ \\ '_ \\ / _ \\ '__/ _` | __| |/ _ \\| '_ \\  / _ \\ '_ \\ / _` | | '_ \\ / _ \\ \\/ / _ \\ '__| | | | | | |\r\n    \\  / (_| | | | (_| |  | (_| |  __/ | | |  __/ | | (_| | |_| | (_) | | | ||  __/ | | | (_| | | | | |  __/\\  /  __/ |  | |_| | |_| |\r\n     \\/ \\__,_|_|  \\__,_|   \\__, |\\___|_| |_|\\___|_|  \\__,_|\\__|_|\\___/|_| |_| \\___|_| |_|\\__, |_|_| |_|\\___| \\/ \\___|_|   \\___(_)___/ \r\n                     ______ __/ |                                         ______          __/ |          ______     ______            \r\n                    |______|___/                                         |______|        |___/          |______|   |______|           ");
                Console.WriteLine("1. Generate NPCs");
                Console.WriteLine("2. Generate Guns");
                Console.WriteLine("3. Exit");
                Console.Write("Please choose an option: ");

                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // Ensure Program.RunNPCGenerator() exists and is compatible with async/await or remove await if it's synchronous
                        await Program.RunNPCGenerator();
                        break;
                    case "2":
                        // Prompt for the number of guns to generate
                        Console.Write("Enter the number of guns to generate: ");
                        if (int.TryParse(Console.ReadLine(), out int numberOfGuns) && numberOfGuns > 0)
                        {
                            GunGeneration gunGeneration = new GunGeneration();
                            await gunGeneration.GenerateAndSaveRandomGuns(numberOfGuns); // Use the user-specified number
                            Console.WriteLine($"{numberOfGuns} guns have been generated and saved.");
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a positive integer.");
                        }
                        break;
                    case "3":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine(); // Add a new line for better readability
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(); // Wait for user input before clearing the screen
                }
            }
        }
    }
}
