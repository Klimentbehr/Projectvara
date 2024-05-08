using System;
using System.Collections.Generic;
using System.IO;

namespace Vara_engine_main
{
    public class FoodItemGeneration
    {
        public class FoodItem
        {
            public string Name { get; set; }
            public int HealthRegain { get; set; }
            public Dictionary<string, int> StatBonuses { get; set; }

            public FoodItem(string name, int healthRegain, Dictionary<string, int> statBonuses)
            {
                Name = name;
                HealthRegain = healthRegain;
                StatBonuses = statBonuses;
            }
        }

        public static List<FoodItem> GenerateFoodItems(string[] lines)
        {
            List<FoodItem> foodItems = new List<FoodItem>();

            foreach (string line in lines)
            {
                string[] data = line.Split(':');
                string name = data[0].Trim();
                string[] stats = data[1].Trim().Split(';');
                int healthRegain = 0;
                Dictionary<string, int> statBonuses = new Dictionary<string, int>();

                foreach (string stat in stats)
                {
                    string[] statData = stat.Split('=');
                    string statName = statData[0].Trim();
                    int statValue = int.Parse(statData[1].Trim());

                    if (statName.Equals("Health", StringComparison.OrdinalIgnoreCase))
                    {
                        healthRegain = statValue;
                    }
                    else
                    {
                        statBonuses.Add(statName, statValue);
                    }
                }

                FoodItem foodItem = new FoodItem(name, healthRegain, statBonuses);
                foodItems.Add(foodItem);
            }

            return foodItems;
        }
    }
}
