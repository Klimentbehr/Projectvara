class Program
{
    static void Main(string[] args)
    {
        WorldGenerator generator = new WorldGenerator();

        // Generate a list of 5 planets
        var planets = generator.GeneratePlanets(5);
        Console.WriteLine("Generated Planets:");
        foreach (var planet in planets)
        {
            Console.WriteLine($"Planet with {planet.Grid.Count}x{planet.Grid[0].Count} grid and size {planet.Size}");
        }

        // Generate a list of 3 solar systems
        var solarSystems = generator.GenerateSolarSystems(3);
        Console.WriteLine("\nGenerated Solar Systems:");
        foreach (var system in solarSystems)
        {
            Console.WriteLine($"Solar system with size {system.Size}");
        }

        // Keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
