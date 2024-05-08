using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NPCGenerator
{
    public class Program
    {
        public static List<string> familyNames;
        public static NameGenerator nameGenerator;
        public static List<NPC> deadNPCs = new List<NPC>();

        public static async Task RunNPCGenerator()
        {
            Console.WriteLine("Enter the number of generations to simulate:");
            if (!int.TryParse(Console.ReadLine(), out int generations))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            // Initialize familyNames list
            familyNames = LoadFamilyNamesFromFile(@"E:\Projectvara\Generation engines\Vara_engine\Generation_engine_REQUIRED_files\last_names.txt");

            nameGenerator = new NameGenerator(@"E:\Projectvara\Generation engines\Vara_engine\Generation_engine_REQUIRED_files\first_names.txt", @"E:\Projectvara\Generation engines\Vara_engine\Generation_engine_REQUIRED_files\last_names.txt");

            List<NPC> allNPCs = new List<NPC>();
            List<NPC> currentGeneration = new List<NPC>();

            // Starting with 20 families, each with 1 NPC
            for (int i = 0; i < 20; i++)
            {
                NPC npc = nameGenerator.GenerateRandomNPC(true); // true indicates max stats
                npc.FamilyName = GetRandomFamilyName();
                allNPCs.Add(npc);
                currentGeneration.Add(npc);
            }

            for (int gen = 0; gen < generations; gen++)
            {
                List<NPC> nextGeneration = new List<NPC>();

                // Age the current NPCs that are not part of the new generation
                foreach (var npc in allNPCs.Where(n => !currentGeneration.Contains(n)))
                {
                    npc.Stats.Age += 45; // Aging by 45 years
                    if (npc.Stats.Age >= 200)
                    {
                        deadNPCs.Add(npc);
                    }
                    else if (npc.Stats.Age >= 25) // NPC starts a family
                    {
                        for (int i = 0; i < new Random().Next(1, 11); i++) // Each family can have 1-10 offspring
                        {
                            NPC childNPC = GenerateChildNPC(npc);
                            nextGeneration.Add(childNPC);
                        }
                    }
                }

                foreach (var npc in currentGeneration)
                {
                    // Determine if NPC can reproduce
                    if (npc.Stats.Age >= 18 && npc.Stats.Age <= 50) // Assuming reproductive age range
                    {
                        int childrenCount = new Random().Next(1, 11); // Randomly generate offspring count between 1 and 10

                        for (int childIndex = 0; childIndex < childrenCount; childIndex++)
                        {
                            Stats childStats = new Stats(
                                Math.Max(0, npc.Stats.Health + new Random().Next(-20, 21)),
                                Math.Max(0, npc.Stats.Dexterity + new Random().Next(-20, 21)),
                                Math.Max(0, npc.Stats.Intelligence + new Random().Next(-20, 21)),
                                Math.Max(0, npc.Stats.Wisdom + new Random().Next(-20, 21)),
                                Math.Max(0, npc.Stats.Charisma + new Random().Next(-20, 21)),
                                Math.Max(0, npc.Stats.Strength + new Random().Next(-20, 21)),
                                1 // Child age starts at 1
                            );

                            NPC childNPC = new NPC(nameGenerator.GenerateFullName(), childStats, npc.Gender, npc.FamilyName, npc.Role);
                            nextGeneration.Add(childNPC);
                        }
                    }
                }

                currentGeneration.Clear();
                currentGeneration.AddRange(nextGeneration);
                allNPCs.AddRange(nextGeneration);
            }

            // Save dead NPCs to a file
            await SaveNPCsToFile(deadNPCs, "Dead_NPCs.txt");
            Console.WriteLine($"{deadNPCs.Count} NPCs have died during the simulation and saved to Dead_NPCs.txt");

            // Save all NPCs to a file
            await SaveNPCsToFile(allNPCs, "All_NPCs.txt");
            Console.WriteLine($"{allNPCs.Count} NPCs have been generated across {generations} generations and saved to All_NPCs.txt");
        }

        private static NPC GenerateChildNPC(NPC parent)
        {
            Stats childStats = new Stats(
                Math.Max(0, parent.Stats.Health + new Random().Next(-20, 21)),
                Math.Max(0, parent.Stats.Dexterity + new Random().Next(-20, 21)),
                Math.Max(0, parent.Stats.Intelligence + new Random().Next(-20, 21)),
                Math.Max(0, parent.Stats.Wisdom + new Random().Next(-20, 21)),
                Math.Max(0, parent.Stats.Charisma + new Random().Next(-20, 21)),
                Math.Max(0, parent.Stats.Strength + new Random().Next(-20, 21)),
                1 // Child age starts at 1
            );

            return new NPC(nameGenerator.GenerateFullName(), childStats, parent.Gender, parent.FamilyName, parent.Role);
        }

        private static List<string> LoadFamilyNamesFromFile(string fileName)
        {
            List<string> names = new List<string>();

            try
            {
                string[] lines = File.ReadAllLines(fileName);
                names.AddRange(lines);
            }
            catch (IOException e)
            {
                Console.WriteLine($"Error reading family names from file: {e.Message}");
            }

            return names;
        }

        private static string GetRandomFamilyName()
        {
            return familyNames[new Random().Next(0, familyNames.Count)];
        }

        private static async Task SaveNPCsToFile(List<NPC> npcs, string fileName)
        {
            string directoryPath = @"E:\Projectvara\Generation engines\Vara_engine\Generated_files\";
            Directory.CreateDirectory(directoryPath); // Ensure directory exists
            string fullPath = Path.Combine(directoryPath, fileName);

            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                foreach (var npc in npcs)
                {
                    await writer.WriteLineAsync(npc.ToString());
                }
            }
        }
    }

    public class NameGenerator
    {
        private Random random = new Random();
        private List<string> firstNames = new List<string>();
        private List<string> lastNames = new List<string>();

        public NameGenerator(string firstNamesFilePath, string lastNamesFilePath)
        {
            LoadNamesFromFile(firstNamesFilePath, firstNames);
            LoadNamesFromFile(lastNamesFilePath, lastNames);
        }

        private void LoadNamesFromFile(string filePath, List<string> namesList)
        {
            try
            {
                string[] names = File.ReadAllLines(filePath);
                namesList.AddRange(names);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading file: {ex.Message}");
            }
        }

        public string GenerateFullName()
        {
            string firstName = firstNames[random.Next(firstNames.Count)];
            string lastName = lastNames[random.Next(lastNames.Count)];
            return $"{firstName} {lastName}";
        }

        public NPC GenerateRandomNPC(bool maxStats)
        {
            string fullName = GenerateFullName();
            Stats stats = maxStats ? new Stats(100, 100, 100, 100, 100, 100, random.Next(20, 61)) :
                new Stats(random.Next(100), random.Next(100), random.Next(100), random.Next(100), random.Next(100), random.Next(100), random.Next(1, 101));
            Gender gender = (Gender)random.Next(2);
            Role role = GetRandomRoleByRarity(); // Use a method to get a role based on rarity
            string familyName = Program.familyNames[random.Next(Program.familyNames.Count)]; // Use familyNames list from Program class
            return new NPC(fullName, stats, gender, familyName, role);
        }

        private Role GetRandomRoleByRarity()
        {
            int randomNumber = random.Next(100); // Generate a number between 0 and 99
            if (randomNumber < 1) // 5% chance for Leadership
            {
                return Role.Leadership;
            }
            else if (randomNumber < 29) // 20% chance for Warrior, since it's uncommon
            {
                return Role.Warrior;
            }
            else // 75% chance for Worker, making it the most common role
            {
                return Role.Worker;
            }
        }
    }

    public class NPC
    {
        public string Name { get; set; }
        public Stats Stats { get; set; }
        public Gender Gender { get; set; }
        public string FamilyName { get; set; }
        public Role Role { get; set; } // Add Role property

        public NPC(string name, Stats stats, Gender gender, string familyName, Role role)
        {
            Name = name;
            Stats = stats;
            Gender = gender;
            FamilyName = familyName;
            Role = role;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Gender: {Gender}, FamilyName: {FamilyName}, Role: {Role}, Stats: {Stats}";
        }
    }

    public class Stats
    {
        public int Health { get; set; }
        public int Dexterity { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }
        public int Strength { get; set; }
        public int Age { get; set; }

        public Stats(int health, int dexterity, int intelligence, int wisdom, int charisma, int strength, int age)
        {
            Health = health;
            Dexterity = dexterity;
            Intelligence = intelligence;
            Wisdom = wisdom;
            Charisma = charisma;
            Strength = strength;
            Age = age;
        }

        public override string ToString()
        {
            return $"Health: {Health}, Dexterity: {Dexterity}, Intelligence: {Intelligence}, Wisdom: {Wisdom}, Charisma: {Charisma}, Strength: {Strength}, Age: {Age}";
        }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public enum Role
    {
        Worker,
        Leadership,
        Warrior
    }
}
