using System.Text.Json;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class Database
    {
        static (List<string>, List<string>, List<string>, List<float>, List<float>, List<bool>)? OpenDatabse(string filepath)
        {
            //Check if file is valid
            if (!File.Exists(filepath) && Path.GetExtension(filepath) != ".json") return null;

            // Make the Lists
            List<string> Names = new();
            List<string> GetsEatenBy = new();
            List<string> Eats = new();
            List<float> Quantity = new();
            List<float> EatsHowMany = new();
            List<bool> FoodOrEater = new();
            
            //Read The File
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(filepath);
            }
            catch
            {
                return null;
            }
            var allData = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(jsonString);

            //Put it into the lists
            foreach (var itemList in allData)
            {
                foreach (var item in itemList)
                {
                    if (item.Key == "name") Names.Add(Convert.ToString(item.Value));
                    if (item.Key == "getseatenby") GetsEatenBy.Add(Convert.ToString(item.Value));
                    if (item.Key == "eats") Eats.Add(Convert.ToString(item.Value));
                    if (item.Key == "quantity") Quantity.Add((float)Convert.ToDecimal(item.Value));
                    if (item.Key == "eatshowmany") EatsHowMany.Add((float)Convert.ToDecimal(item.Value));
                    if (item.Key == "foodoreater") FoodOrEater.Add(Convert.ToBoolean(item.Value));
                }
            }
            // Return
            return (Names, GetsEatenBy, Eats, Quantity, EatsHowMany, FoodOrEater);
        }

        static void SaveToDatabase(List<string> Names, List<string> GetsEatenBy, List<string> Eats,
            List<float> Quantity, List<float> EatsHowMany, List<bool> FoodOrEater, string filepath)
        {
            //Check if file is valid
            if (!File.Exists(filepath) && Path.GetExtension(filepath) != ".json") return;

            //Convert To json
            var allData = new List<Dictionary<string, object>>();
            for (int i = 0; i > Names.Count(); i++)
            {
                var data = new Dictionary<string, object>
                {
                    { "name", Names[i] },
                    { "getseatenby", GetsEatenBy[i] },
                    { "eats", Eats[i] },
                    {"quantity", Quantity[i]},
                    {"eatshowmany", EatsHowMany[i]},
                    {"foodoreater", FoodOrEater[i]},
                };
                allData.Add(data);
            }
            string jsonString = JsonSerializer.Serialize(allData, new JsonSerializerOptions { WriteIndented = true });

            //Save it
            try
            {
                File.WriteAllText(filepath, jsonString);
            }
            catch
            {
                return;
            }
        }
    }
}