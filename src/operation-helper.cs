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

        public class SpeciesData
        {
            public string Name { get; set; }
            public List<float> DailyPopulations { get; set; }

            public SpeciesData(string name)
            {
                Name = name;
                DailyPopulations = new List<float>();
            }
        }

        public static (List<float>, List<SpeciesData>) Simulate(List<string> names, List<string> eats,
            List<float> quantity,
            List<float> eatsHowMany, List<float> deathsPerDay, List<float> replication, List<float> multiplier,
            int days)
        {
            Dictionary<string, float> population = new Dictionary<string, float>();
            List<SpeciesData> allSpeciesData = names.Select(name => new SpeciesData(name)).ToList();
            for (int i = 0; i < names.Count; i++)
            {
                population[names[i]] = quantity[i];
            }

            Random random = new Random();

            (var layerIndexes, var layerBoundaries) = OperationHelper.SortByLayer(names, eats);

            for (int day = 0; day < days; day++)
            {
                Dictionary<string, float> newPopulation = new Dictionary<string, float>(population);

                foreach (var idx in layerIndexes)
                {
                    string species = names[idx];
                    float currentPopulation = population[species];

                    if (currentPopulation <= 0)
                    {
                        allSpeciesData[idx].DailyPopulations.Add(0); 
                        continue;
                    }

                    float birthRate = replication[idx];
                    float deathRate = deathsPerDay[idx];
                    float variationMultiplier = multiplier[idx];

                    float totalFoodConsumed = 0f;
                    if (!string.IsNullOrEmpty(eats[idx]))
                    {
                        string[] preySpecies = eats[idx].Split(',');
                        foreach (string prey in preySpecies)
                        {
                            if (population.ContainsKey(prey.Trim()))
                            {
                                float availableFood = population[prey.Trim()];
                                float foodConsumed = CalculateEventOutcome(Math.Min(eatsHowMany[idx], availableFood),
                                    variationMultiplier, random);
                                totalFoodConsumed += foodConsumed;
                                newPopulation[prey.Trim()] = Math.Max(0, newPopulation[prey.Trim()] - foodConsumed);
                            }
                        }

                        float survivalRate = Math.Min(1f, totalFoodConsumed / eatsHowMany[idx]);
                        float survivingPopulation = currentPopulation * survivalRate;

                        float reproductionOutcome = CalculateEventOutcome(birthRate * survivingPopulation,
                            variationMultiplier, random);
                        newPopulation[species] = survivingPopulation + reproductionOutcome;
                    }
                    else
                    {
                        float reproductionOutcome =
                            CalculateEventOutcome(birthRate * currentPopulation, variationMultiplier, random);
                        newPopulation[species] += reproductionOutcome;
                    }

                    float deathOutcome =
                        CalculateEventOutcome(deathRate * newPopulation[species], variationMultiplier, random);
                    newPopulation[species] = Math.Max(0, newPopulation[species] - deathOutcome);

                    allSpeciesData[idx].DailyPopulations.Add(newPopulation[species]);
                }

                population = new Dictionary<string, float>(newPopulation);
            }

            List<float> finalPopulations = population.Values.ToList();
            return (finalPopulations, allSpeciesData);
        }

        private static float CalculateProbability(float multiplier)
        {
            return Math.Clamp(multiplier, 0f, 1f);
        }

        private static float CalculateEventOutcome(float eventAmount, float multiplier, Random random)
        {
            if (random.NextDouble() < multiplier)
            {
                return eventAmount;
            }
            else
            {
                double partialMultiplier = Math.Pow(random.NextDouble(), 2);
                return (float)(eventAmount * partialMultiplier);
            }
        }
    }
}