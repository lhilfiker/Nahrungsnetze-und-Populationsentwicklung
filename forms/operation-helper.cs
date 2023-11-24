using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class OperationHelper
    {
        public static (List<int> LayerIndexes, List<int> LayerBoundaries) SortByLayer(
            List<string> names, List<string> getsEatenBy, List<string> eats, List<bool> foodOrEater)
        {
            List<int> AllItems = new List<int>();
            List<int> LayerEndings = new List<int>();
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

        public static List<int> GetLayer(List<int> AllItems, List<int> LayerEndings, int Layer)
        {
            List<int> LayerRequested = new List<int>();
            if (AllItems == null || LayerEndings == null || AllItems.Count == 0 || LayerEndings.Count == 0)
                return LayerRequested;

            if (Layer < 1 || Layer > LayerEndings.Count) return LayerRequested;

            int start = Layer == 1 ? 0 : LayerEndings[Layer - 2];
            int end = LayerEndings[Layer - 1];

            if (start < 0 || end > AllItems.Count) return LayerRequested;

            for (int i = start; i < end; i++)
            {
                LayerRequested.Add(AllItems[i]);
            }

            return LayerRequested;
        }

        public static (bool, string) ValueValidation(List<string> names, List<string> getsEatenBy, List<string> eats, List<bool> foodOrEater)
        {
            foreach (var item in getsEatenBy)
            {
                if (!names.Contains(item))
                {
                    return (false, $"{item} doesnt exist in Names. This is not valid.");
                }
            }
            foreach (var item in eats)
            {
                if (!names.Contains(item))
                {
                    return (false, $"{item} doesnt exist in eats. This is not valid.");
                }
            }
            foreach (var item in eats)
            {
                if (!names.Contains(item))
                {
                    return (false, $"{item} doesnt exist in eats. This is not valid.");
                }
            }

            int i = 0;
            foreach (var item in foodOrEater)
            {
                if (item && eats[i] != "" )
                {
                    return (false, $"{names[i]} can not be Food and Eat something.");
                }

                i++;
            }
            
            return (true, "all good.");
        }

    }
}