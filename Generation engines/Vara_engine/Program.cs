using Main;
using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Vara_engine_main;

namespace NPCGenerator
{
    class EntryPoint
    {
        static async Task Main(string[] args)
        {
            bool exit = false;
            WorldGenerator generator = new WorldGenerator();

            while (!exit)
            {
                Console.Clear();
                Console.WriteLine("Game Utilities Menu");
                Console.WriteLine("1. Generate NPCs");
                Console.WriteLine("2. Generate Guns");
                Console.WriteLine("3. Generate Food Items");
                Console.WriteLine("4. Generate Game World");
                Console.WriteLine("5. Exit");
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
                        foreach (var foodItem in foodItems)
                        {
                            Console.WriteLine($"Name: {foodItem.Name}, Health Regain: {foodItem.HealthRegain}");
                            Console.WriteLine("Stat Bonuses:");
                            foreach (var statBonus in foodItem.StatBonuses)
                            {
                                Console.WriteLine($"{statBonus.Key}: +{statBonus.Value}");
                            }
                            Console.WriteLine();
                        }
                        break;
                    case "4":
                        var planets = generator.GeneratePlanets(5);
                        var solarSystems = generator.GenerateSolarSystems(3);
                        SaveWorldData(planets, "E:\\Projectvara\\Generation engines\\Vara_engine\\Generated_files\\Planets.json");
                        SaveWorldData(solarSystems, "E:\\Projectvara\\Generation engines\\Vara_engine\\Generated_files\\SolarSystems.json");
                        Console.WriteLine("World generation complete.");
                        Console.WriteLine($"Generated {planets.Count} planets and {solarSystems.Count} solar systems.");
                        break;
                    case "5":
                        exit = true;
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

        static void SaveWorldData<T>(List<T> data, string filePath)
        {
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            // Ensure the directory exists
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllText(filePath, json);
            Console.WriteLine($"Data saved to {filePath}");
        }
    }
}
