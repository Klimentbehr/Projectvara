using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Main
{
    internal class GunGeneration
    {
        private Random random = new Random();
        private string directoryPath = @"E:\Projectvara\Generation engines\Vara_engine\Generated_files\";

        public enum GunOrigin
        {
            WarsawPact,
            NATO
        }

        private Dictionary<GunOrigin, List<string>> ammoTypes = new Dictionary<GunOrigin, List<string>>
        {
            { GunOrigin.WarsawPact, new List<string> { "7.62x39mm", "7.62x54mmR", "5.45x39mm" } },
            { GunOrigin.NATO, new List<string> { "5.56x45mm NATO", "7.62x51mm NATO", "9mm NATO" } }
        };

        public class Gun
        {
            public string Name { get; set; }
            public GunOrigin Origin { get; set; }
            public string UpperReceiver { get; set; }
            public string Barrel { get; set; }
            public string LowerReceiver { get; set; }
            public string BufferTube { get; set; }
            public string Stock { get; set; }
            public string Grip { get; set; }
            public string Trigger { get; set; }
            public string AmmoType { get; set; }

            public Gun(string name, GunOrigin origin, string ammoType,
                string upperReceiver, string barrel, string lowerReceiver,
                string bufferTube, string stock, string grip, string trigger)
            {
                Name = name;
                Origin = origin;
                AmmoType = ammoType;
                UpperReceiver = upperReceiver;
                Barrel = barrel;
                LowerReceiver = lowerReceiver;
                BufferTube = bufferTube;
                Stock = stock;
                Grip = grip;
                Trigger = trigger;
            }

            public override string ToString()
            {
                return $"Name: {Name}\n" +
                       $"Origin: {Origin}\n" +
                       $"Ammo Type: {AmmoType}\n" +
                       $"Upper Receiver: {UpperReceiver}\n" +
                       $"Barrel: {Barrel}\n" +
                       $"Lower Receiver: {LowerReceiver}\n" +
                       $"Buffer Tube: {BufferTube}\n" +
                       $"Stock: {Stock}\n" +
                       $"Grip: {Grip}\n" +
                       $"Trigger: {Trigger}";
            }
        }

        private List<string> warsawPactGuns = new List<string> { "AK-47", "SKS", "RPD" };
        private List<string> natoGuns = new List<string> { "M16A4", "FN SCAR", "M4A1" };

        private string SelectQualityDescriptor(string partName)
        {
            List<string> qualityDescriptors = new List<string> { "Poor", "Standard", "Superior", "First-rate" };
            int index = random.Next(qualityDescriptors.Count);
            return $"{qualityDescriptors[index]} {partName}";
        }

        public Gun GenerateRandomGun()
        {
            GunOrigin origin = (GunOrigin)random.Next(2);
            string name = origin == GunOrigin.WarsawPact ? warsawPactGuns[random.Next(warsawPactGuns.Count)] : natoGuns[random.Next(natoGuns.Count)];
            string ammoType = ammoTypes[origin][random.Next(ammoTypes[origin].Count)];

            return new Gun(name, origin, ammoType,
                SelectQualityDescriptor("Upper Receiver"),
                SelectQualityDescriptor("Barrel"),
                SelectQualityDescriptor("Lower Receiver"),
                SelectQualityDescriptor("Buffer Tube"),
                SelectQualityDescriptor("Stock"),
                SelectQualityDescriptor("Grip"),
                SelectQualityDescriptor("Trigger"));
        }

        public async Task GenerateAndSaveRandomGuns(int numberOfGuns)
        {
            List<Gun> guns = new List<Gun>();
            for (int i = 0; i < numberOfGuns; i++)
            {
                guns.Add(GenerateRandomGun());
            }

            await SaveGunsToFile(guns, "Guns.txt");
        }

        private async Task SaveGunsToFile(List<Gun> guns, string fileName)
        {
            string filePath = Path.Combine(directoryPath, fileName);

            Directory.CreateDirectory(directoryPath); // Ensure the directory exists

            using StreamWriter file = new StreamWriter(filePath);
            foreach (var gun in guns)
            {
                await file.WriteLineAsync(gun.ToString());
                await file.WriteLineAsync(); // Add an empty line for readability between gun entries
            }

            Console.WriteLine($"{guns.Count} guns have been generated and saved to {filePath}");
        }
    }
}
