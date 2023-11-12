using System;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class test
    {
        public static void TestCase()
        {

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
            
            Console.WriteLine("Saved. I will try to load it now.");
            
            
            
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
                var (layerOne, layerTwo) = sortedLayers.Value;

                // Display the results
                Console.WriteLine("Layer One:");
                foreach (var item in layerOne)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\nLayer Two:");
                foreach (var item in layerTwo)
                {
                    Console.WriteLine(item);
                }
            }
            else
            {
                Console.WriteLine("Sorting failed or returned no data.");
            }

            


        }
    }
}