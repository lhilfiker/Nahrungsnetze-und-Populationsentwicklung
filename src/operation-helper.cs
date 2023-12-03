using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Nahrungsnetze_und_Populationsentwicklung
{
    internal class OperationHelper
    {
        public static (List<int> LayerIndexes, List<int> LayerBoundaries) SortByLayer(
            List<string> names, List<string> eats)
        {
            List<int> layerIndexes = new List<int>();
            List<int> layerBoundaries = new List<int>();
            HashSet<int> remaining = new HashSet<int>(Enumerable.Range(0, names.Count));

            // First Layer - Producers or those who don't eat anything
            for (int i = 0; i < names.Count; i++)
            {
                if (string.IsNullOrEmpty(eats[i]))
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
                    var eatsList = eats[index].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Select(e => e.Trim()).ToList();

                    // Check if the current entity is eaten by any remaining entity or eats any remaining entity
                    bool isEatenOrEatsRemaining = remaining.Any(rem => eatsList.Contains(names[rem]));

                    if (eatsList.Any() && !isEatenOrEatsRemaining)
                    {
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

        private static Random random = new Random();

        public static List<float> Simulate(List<string> names, List<string> eats, List<float> quantity,
            List<float> eatsHowMany, List<float> deathsPerDay, List<float> replication, List<float> multiplier,
            int days)
        {
            Dictionary<string, float> population = new Dictionary<string, float>();
            for (int i = 0; i < names.Count; i++)
            {
                population[names[i]] = quantity[i];
            }

            for (int day = 0; day < days; day++)
            {
                Dictionary<string, float> newPopulation = new Dictionary<string, float>(population);

                for (int i = 0; i < names.Count; i++)
                {
                    string species = names[i];
                    float currentPopulation = population[species];
                    float birthRate = replication[i];
                    float deathRate = deathsPerDay[i];
                    float variationMultiplier = multiplier[i];

                    // Wahrscheinlichkeit für das Ereignis berechnen
                    float probability = CalculateProbability(variationMultiplier);

                    if (!string.IsNullOrEmpty(eats[i]) && population.ContainsKey(eats[i]))
                    {
                        float availableFood = population[eats[i]];
                        float foodConsumed = Math.Min(eatsHowMany[i], availableFood);
                        if (random.NextDouble() < probability)
                        {
                            // Nahrungsaufnahme und Fortpflanzung
                            float reproductionRate = (foodConsumed / eatsHowMany[i]) * birthRate;
                            newPopulation[eats[i]] -= foodConsumed;
                            newPopulation[species] += reproductionRate * currentPopulation;
                        }
                    }
                    else
                    {
                        if (random.NextDouble() < probability)
                        {
                            // Fortpflanzung für Produzenten
                            newPopulation[species] += birthRate * currentPopulation;
                        }
                    }

                    if (random.NextDouble() < probability)
                    {
                        // Tod
                        newPopulation[species] -= deathRate * currentPopulation;
                    }

                    newPopulation[species] = Math.Max(0, newPopulation[species]);
                }

                population = newPopulation;
            }

            List<float> finalQuantities = new List<float>();
            foreach (var species in names)
            {
                finalQuantities.Add(population[species]);
            }

            return finalQuantities;
        }

        private static float CalculateProbability(float multiplier)
        {
            // Implementiere eine exponentielle Skalierung für die Wahrscheinlichkeit
            // Multiplier-Werte reichen von 0 bis 100
            if (multiplier >= 100) return 1.0f;
            if (multiplier <= 0) return 1 / 1000000.0f; // Sehr geringe Wahrscheinlichkeit für 0

            // Exponentielle Skalierung
            return (float)Math.Pow(10, multiplier / 100 - 1);
        }
    }
}