using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace NPCGenerator
{
    public class Program
    {
        public static List<string> familyNames = new List<string> { "Smith", "Johnson", "Williams", "Jones", "Brown", "Davis", "Miller", "Wilson" };
        public static NameGenerator nameGenerator = new NameGenerator();
        public static Random random = new Random();

        public static async Task RunNPCGenerator()
        {
            Console.WriteLine("Enter the number of generations to simulate:");
            if (!int.TryParse(Console.ReadLine(), out int generations))
            {
                Console.WriteLine("Invalid input. Please enter a valid number.");
                return;
            }

            List<NPC> allNPCs = new List<NPC>();
            for (int i = 0; i < 2; i++) // Starting with 2 NPCs
            {
                allNPCs.Add(nameGenerator.GenerateRandomNPC(true)); // true indicates max stats
            }

            for (int gen = 0; gen < generations; gen++)
            {
                List<NPC> nextGeneration = new List<NPC>();
                foreach (var npc in allNPCs)
                {
                    nextGeneration.AddRange(npc.GenerateChildren(nameGenerator, random));
                }
                allNPCs.AddRange(nextGeneration);
            }

            await SaveNPCsToFile(allNPCs, "NPCs.txt");
            Console.WriteLine($"{allNPCs.Count} NPCs have been generated across {generations} generations and saved to NPCs.txt");
        }

        static async Task SaveNPCsToFile(List<NPC> npcs, string filePath)
        {
            string directoryPath = @"E:\Projectvara\Generation engines\Vara_engine\Generated_files\";
            Directory.CreateDirectory(directoryPath); // Ensure directory exists
            string fullPath = Path.Combine(directoryPath, filePath);

            using StreamWriter file = new StreamWriter(fullPath);
            foreach (var npc in npcs)
            {
                await file.WriteLineAsync(npc.ToString());
            }
        }
    }

    public class NameGenerator
    {
        private Random random = new Random();
        private List<string> firstNames = new List<string>
        {
            "Emma", "Liam", "Olivia", "Noah", "Ava", "William", "Sophia", "James", "Isabella", "Logan",
            "Charlotte", "Benjamin", "Mia", "Elijah", "Amelia", "Oliver", "Harper", "Jacob", "Evelyn", "Lucas"
        };
        private List<string> lastNames = new List<string>
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin"
        };

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
            string familyName = lastNames[random.Next(lastNames.Count)];
            return new NPC(fullName, stats, gender, familyName, role);
        }
        private Role GetRandomRoleByRarity()
        {
            int randomNumber = random.Next(100); // Generate a number between 0 and 99
            if (randomNumber < 5) // 5% chance for Leadership
            {
                return Role.Leadership;
            }
            else if (randomNumber < 25) // 20% chance for Warrior, since it's uncommon
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

        public List<NPC> GenerateChildren(NameGenerator nameGenerator, Random random)
        {
            int childrenCount = random.Next(1, 4); // Each NPC can have 1 to 3 children
            List<NPC> children = new List<NPC>();
            for (int i = 0; i < childrenCount; i++)
            {
                Stats childStats = new Stats(
                    Math.Max(0, Stats.Health + random.Next(-20, 21)),
                    Math.Max(0, Stats.Dexterity + random.Next(-20, 21)),
                    Math.Max(0, Stats.Intelligence + random.Next(-20, 21)),
                    Math.Max(0, Stats.Wisdom + random.Next(-20, 21)),
                    Math.Max(0, Stats.Charisma + random.Next(-20, 21)),
                    Math.Max(0, Stats.Strength + random.Next(-20, 21)),
                    random.Next(1, 101) // Child age is randomly generated
                );
                children.Add(new NPC(nameGenerator.GenerateFullName(), childStats, Gender, FamilyName, (Role)random.Next(Enum.GetNames(typeof(Role)).Length))); // Assign random role to children
            }
            return children;
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
