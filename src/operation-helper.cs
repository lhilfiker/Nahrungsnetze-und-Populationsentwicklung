using System;
using System.Collections.Generic;
using System.Linq;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class OperationHelper
    {
        public static (List<int> LayerIndexes, List<int> LayerBoundaries) SortByLayer(
            List<string> names, List<string> getsEatenBy, List<string> eats, List<bool> foodOrEater)
        {
            List<int> AllItems = new();
            List<int> LayerEndings = new();
            List<int> Remaining = Enumerable.Range(0, names.Count).ToList();
            
            // Layer 1
            for (int i = 0; i < names.Count; i++)
            {
                if (eats[i] == "" || foodOrEater[i])
                {
                    AllItems.Add(i);
                    Remaining.Remove(i);
                }
            }
            LayerEndings.Add(AllItems.Count);
            
            // Other Layers
            int iterations = 0;
            while (iterations < 1000 && Remaining.Any())
            {
                List<int> toRemove = new List<int>();
                foreach (int item in Remaining)
                {
                    bool EatsSomething = AllItems.Any(food => getsEatenBy[food] == names[item] || eats[item] == names[food]);
                    bool WillItEatSomething = Remaining.Any(food => getsEatenBy[food] == names[item]);

                    if (!WillItEatSomething && EatsSomething)
                    {
                        toRemove.Add(item);
                        AllItems.Add(item);
                    }
                }

                foreach (var item in toRemove)
                {
                    Remaining.Remove(item);
                }

                LayerEndings.Add(AllItems.Count);
                iterations++;
            }
            
            return (AllItems, LayerEndings);
        }
    }
}