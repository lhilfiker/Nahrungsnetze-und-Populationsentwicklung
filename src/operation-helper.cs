

using System.Runtime.CompilerServices;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class OperationHelper
    {


        public static (List<int>, List<int>)? SortByLayer(List<string> Names, List<string> GetsEatenBy, List<string> Eats,
            List<float> Quantity, List<float> EatsHowMany, List<bool> FoodOrEater
            )
        {
            int ItemsAdded = 0;
            List<int> LatestFood = new();
            List<int> AlreadyAdded = new();
            List<int> TempLast = new();
            List<int> LayersEnd = new();
            while (ItemsAdded != Names.Count())
            {
                if (LatestFood.Count() == 0)
                {
                    int i = 0;
                    foreach (var item in FoodOrEater)
                    {
                        if (item)
                        {
                            LatestFood.Add(i);
                            AlreadyAdded.Add(i);
                            ItemsAdded++;
                        }

                        i++;
                    }
                }
                else
                {
                    int i = -1;
                    TempLast.Clear();
                    foreach (var item in Eats)
                    {
                        i++;
                        if (FoodOrEater[i]) continue;
                        if (AlreadyAdded.Contains(i)) continue;
                        
                        if(LatestFood.Contains(Names.IndexOf(item)))
                        {
                            TempLast.Add(i);
                            AlreadyAdded.Add(i);
                            ItemsAdded++;
                        }
                    }

                    LatestFood.Clear();
                    foreach (var item in TempLast)
                    {
                        LatestFood.Add(item);
                    }

                    TempLast.Clear();
                }
                LayersEnd.Add(AlreadyAdded.Last());
            }
            return (AlreadyAdded, LayersEnd);            
        }
        
        static (List<string>, List<string>, List<string>, List<float>, List<float>, List<bool>)? Default(List<string> Names, List<string> GetsEatenBy, List<string> Eats,
            List<float> Quantity, List<float> EatsHowMany, List<bool> FoodOrEater
        )
        {
            return null;
        }
    }
}
