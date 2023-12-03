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
            List<int> layerIndexes = new List<int>();
            List<int> layerBoundaries = new List<int>();
            HashSet<int> remaining = new HashSet<int>(Enumerable.Range(0, names.Count));

            // First Layer - Producers or those who don't eat anything
            for (int i = 0; i < names.Count; i++)
            {
                if (eats[i] == "" || foodOrEater[i])
                {
                    layerIndexes.Add(i);
                    remaining.Remove(i);
                }
            }

            layerBoundaries.Add(layerIndexes.Count);

            // Subsequent Layers
            bool addedToLayer;
            do
            {
                addedToLayer = false;
                List<int> toAdd = new List<int>();

                foreach (int index in remaining)
                {
                    string eatsItem = eats[index];
                    bool isEatenByRemaining = remaining.Any(rem => getsEatenBy[rem] == names[index]);
                    bool eatsRemaining = remaining.Any(rem => eats[index] == names[rem]);

                    if (eatsItem != "" && !isEatenByRemaining && !eatsRemaining)
                    {
                        // This entity eats something and is not eaten by any remaining entity,
                        // and does not eat any remaining entity
                        toAdd.Add(index);
                    }
                }

                foreach (int index in toAdd)
                {
                    layerIndexes.Add(index);
                    remaining.Remove(index);
                    addedToLayer = true;
                }

                if (addedToLayer)
                {
                    layerBoundaries.Add(layerIndexes.Count);
                }
            } while (addedToLayer);

            return (layerIndexes, layerBoundaries);
        }


        public static List<int> GetLayer(List<int> AllItems, List<int> LayerEndings, int Layer)
        {
            List<int> LayerRequested = new();
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

        public static (bool, string) ValueValidation(List<string> names, List<string> getsEatenBy, List<string> eats,
            List<bool> foodOrEater)
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
                if (item && eats[i] != "")
                {
                    return (false, $"{names[i]} can not be Food and Eat something.");
                }

                i++;
            }

            return (true, "all good.");
        }
    }
}