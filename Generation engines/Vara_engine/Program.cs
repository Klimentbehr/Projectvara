using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace NPCGenerator
{
    class EntryPoint
    {
        static async Task Main(string[] args)
        {
            bool exit = false;
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
                    case "4":
                        var generator = new WorldGenerator();
                        var planets = generator.GeneratePlanets(5);
                        var solarSystems = generator.GenerateSolarSystems(3);
                        Console.WriteLine("World generation complete.");

                        // Serialize and save the generated data
                        var filePath = "GeneratedWorld.json";
                        SaveGeneratedData(planets, solarSystems, filePath);
                        Console.WriteLine($"Data saved to {filePath}");
                        break;
                    case "5":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            }
        }

        static void SaveGeneratedData(List<Planet> planets, List<Hexagon> solarSystems, string filePath)
        {
            var data = new
            {
                Planets = planets,
                SolarSystems = solarSystems
            };
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
