using System;
using System.Collections.Generic;

public class WorldGenerator
{
    Random random = new Random();

    public List<Planet> GeneratePlanets(int numberOfPlanets)
    {
        List<Planet> planets = new List<Planet>();
        for (int i = 0; i < numberOfPlanets; i++)
        {
            planets.Add(new Planet
            {
                Size = random.Next(1, 10), // Random size for simplicity
                Grid = GeneratePlanetGrid(random.Next(5, 10), random.Next(5, 10)) // Random grid size
            });
        }
        return planets;
    }

    private List<List<PlanetSquare>> GeneratePlanetGrid(int width, int height)
    {
        List<List<PlanetSquare>> grid = new List<List<PlanetSquare>>();
        for (int x = 0; x < width; x++)
        {
            List<PlanetSquare> row = new List<PlanetSquare>();
            for (int y = 0; y < height; y++)
            {
                row.Add(new PlanetSquare
                {
                    Type = (SquareType)random.Next(0, 5) // Random type for each square
                });
            }
            grid.Add(row);
        }
        return grid;
    }

    public List<Hexagon> GenerateSolarSystems(int numberOfSystems)
    {
        List<Hexagon> solarSystems = new List<Hexagon>();
        for (int i = 0; i < numberOfSystems; i++)
        {
            solarSystems.Add(new Hexagon
            {
                Size = random.Next(10, 50) // Random size for hexagon area
            });
        }
        return solarSystems;
    }
}

public class Planet
{
    public int Size { get; set; }
    public List<List<PlanetSquare>> Grid { get; set; }
}

public class PlanetSquare
{
    public SquareType Type { get; set; }
}

public enum SquareType
{
    PointsOfInterest, UndergroundResource, SurfaceResource, Empty, City
}

public class Hexagon
{
    public int Size { get; set; }
}

