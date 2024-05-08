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
            Civilian,
            Military
        }

        public class Gun
        {
            public string ID { get; set; }
            public GunOrigin Origin { get; set; }
            public string UpperReceiver { get; set; }
            public string Barrel { get; set; }
            public string LowerReceiver { get; set; }
            public string BufferTube { get; set; }
            public string Stock { get; set; }
            public string Grip { get; set; }
            public string Trigger { get; set; }
            public int Damage { get; set; }
            public int Accuracy { get; set; }

            public Gun(string id, GunOrigin origin,
                string upperReceiver, string barrel, string lowerReceiver,
                string bufferTube, string stock, string grip, string trigger,
                int damage, int accuracy)
            {
                ID = id;
                Origin = origin;
                UpperReceiver = upperReceiver;
                Barrel = barrel;
                LowerReceiver = lowerReceiver;
                BufferTube = bufferTube;
                Stock = stock;
                Grip = grip;
                Trigger = trigger;
                Damage = damage;
                Accuracy = accuracy;
            }

            public override string ToString()
            {
                return $"ID: {ID}\n" +
                       $"Origin: {Origin}\n" +
                       $"Upper Receiver: {UpperReceiver}\n" +
                       $"Barrel: {Barrel}\n" +
                       $"Lower Receiver: {LowerReceiver}\n" +
                       $"Buffer Tube: {BufferTube}\n" +
                       $"Stock: {Stock}\n" +
                       $"Grip: {Grip}\n" +
                       $"Trigger: {Trigger}\n" +
                       $"Damage: {Damage}\n" +
                       $"Accuracy: {Accuracy}";
            }
        }

        private string GeneratePartID()
        {
            return Guid.NewGuid().ToString().Substring(0, 8);
        }

        private string SelectQualityDescriptor()
        {
            List<string> qualityDescriptors = new List<string> { "Poor", "Standard", "Superior", "First-rate" };
            int index = random.Next(qualityDescriptors.Count);
            return qualityDescriptors[index];
        }

        private int DetermineStatBonus(string qualityDescriptor)
        {
            return qualityDescriptor switch
            {
                "Poor" => -5,
                "Standard" => 0,
                "Superior" => 5,
                "First-rate" => 10,
                _ => 0,
            };
        }

        public Gun GenerateRandomGun()
        {
            GunOrigin origin = (GunOrigin)random.Next(2);
            string id = "Gun" + GeneratePartID();

            // Generate parts with IDs and qualities
            string upperReceiver = GeneratePartID() + "." + SelectQualityDescriptor();
            string barrel = GeneratePartID() + "." + SelectQualityDescriptor();
            string trigger = GeneratePartID() + "." + SelectQualityDescriptor();

            // Apply stat bonuses based on quality
            int damage = 50 + DetermineStatBonus(SelectQualityDescriptor()); // Example base damage
            int accuracy = 75 + DetermineStatBonus(SelectQualityDescriptor()); // Example base accuracy

            return new Gun(id, origin,
                upperReceiver,
                barrel,
                GeneratePartID() + "." + SelectQualityDescriptor(),
                GeneratePartID() + "." + SelectQualityDescriptor(),
                GeneratePartID() + "." + SelectQualityDescriptor(),
                GeneratePartID() + "." + SelectQualityDescriptor(),
                trigger,
                damage,
                accuracy);
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
