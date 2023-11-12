using System;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class test
    {
        public static void TestCase()
        {
            
            Console.WriteLine("Nahrungsnetz and Databse Save Load and Sort TEST. A test databse will be created in your desktop folder. Then it will get loaded and sorted. You can change the values in code.\n");

            List<string> Names = new List<string> 
            {
                "Blume", "Käfer", "Spinne", "Kleiner Vogel", "Wurm", 
                "Frosch", "Maus", "Schlange", "Großer Vogel", "Fuchs", 
                "Hase", "Eule", "Hirsch", "Wolf", "Pilz"
            };

            List<string> GetsEatenBy = new List<string> 
            {
                "", "Spinne", "Kleiner Vogel", "Großer Vogel", "Käfer", 
                "Schlange", "Schlange", "Fuchs", "Eule", "Wolf", 
                "Fuchs", "Wolf", "Wolf", "", "Wurm"
            };

            List<string> Eats = new List<string> 
            {
                "", "Blume", "Käfer", "Spinne", "Pilz", 
                "Wurm", "Blume", "Frosch", "Kleiner Vogel", "Hase", 
                "Blume", "Großer Vogel", "Blume", "Hirsch", "Blume"
            };

            List<float> Quantity = new List<float> 
            {
                1000, 500, 300, 200, 600, 
                150, 400, 100, 150, 70, 
                350, 60, 80, 50, 900
            };

            List<float> EatsHowMany = new List<float> 
            {
                0, 0.1f, 0.2f, 0.15f, 0.05f, 
                0.2f, 0.1f, 0.3f, 0.25f, 0.4f, 
                0.1f, 0.3f, 0.5f, 0.6f, 0
            };

            List<bool> FoodOrEater = new List<bool> 
            {
                true, false, false, false, true, 
                false, false, false, false, false, 
                false, false, false, false, true
            };

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testcase.json");

            Database.SaveToDatabase(Names, GetsEatenBy, Eats, Quantity, EatsHowMany, FoodOrEater, path);

            Names.Clear();
            GetsEatenBy.Clear();
            Eats.Clear();
            Quantity.Clear();
            EatsHowMany.Clear();
            FoodOrEater.Clear();
            
            Console.WriteLine("\nSaved. I will try to load it now.\n");
            
            
            
            var result = Database.OpenDatabase(path);
            if (result.HasValue)
            {
                // Assign the values from the result to the lists
                (Names, GetsEatenBy, Eats, Quantity, EatsHowMany, FoodOrEater) = result.Value;
            }

            for (int i = 0; i < Names.Count; i++)
            {
                Console.WriteLine($"Name: {Names[i]}, Gets Eaten By: {GetsEatenBy[i]}, Eats: {Eats[i]}, Quantity: {Quantity[i]}, Eats How Many: {EatsHowMany[i]}, Food or Eater: {FoodOrEater[i]}");
            }
            
            Console.WriteLine("Load/SAVE Finsihed. Will Sort it now.");
            
            // Call the SortByLayer function
            var sortedLayers = OperationHelper.SortByLayer(Names, GetsEatenBy, Eats, Quantity, EatsHowMany, FoodOrEater);

            if (sortedLayers.HasValue)
            {
                var (layerIndexes, layerBoundaries) = sortedLayers.Value;

                int layerStart = 0;
                int layerNumber = 1;

                // Iterate through the LayerBoundaries to display each layer
                foreach (var boundary in layerBoundaries)
                {
                    Console.WriteLine($"Layer {layerNumber}:");
                    for (int i = layerStart; i < boundary; i++)
                    {
                        // Display the name using the index from LayerIndexes
                        Console.WriteLine(Names[layerIndexes[i]]);
                    }
                    layerStart = boundary;
                    layerNumber++;
                    Console.WriteLine(); // For better readability
                }
            }
            else
            {
                Console.WriteLine("Sorting failed or returned no data.");
            }


            


        }
    }
}