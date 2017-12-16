using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public int CurrentGeneration { get; set; }

        public int AmountOfBestGenotypesForParents = SimulationManagerScript.Instance.AmountOfBestGenotypesForParents < 2 ?
                                                    2 : SimulationManagerScript.Instance.AmountOfBestGenotypesForParents;
        private bool keepBestGenotype = true;
        public double MutationProbability = SimulationManagerScript.Instance.MutationProbability;
        public double MutationAmount = SimulationManagerScript.Instance.MutationAmount;
        public static int _CurrentGeneration = 0;

        public bool ExtraRandomizationBeforeMinFitness;
        public int MinFitness = 24;
        public int PercentOfRandomGenotypes = 50;

        public GeneticAlgorithm()
        {
            CurrentGeneration = _CurrentGeneration;
            _CurrentGeneration++;
        }

        public Genotype[] Start(Genotype[] genotypes)
        {
            Genotype[] newGenotypes = BreedNewPopulation(genotypes);
            newGenotypes = MutatePopulation(newGenotypes);

            foreach (var genotype in newGenotypes)
            {
                genotype.fitness = 0;
            }
            return newGenotypes;
        }

        public Genotype[] BreedNewPopulation(Genotype[] previousGenotypes)
        {
            Genotype[] newGenotypes = new Genotype[previousGenotypes.Length];
            Genotype[] bestPreviousGenotypes = SelectBestGenotypes(AmountOfBestGenotypesForParents, previousGenotypes);

            if (bestPreviousGenotypes[0].fitness < MinFitness && CurrentGeneration > 4) ExtraRandomizationBeforeMinFitness = true;
            else ExtraRandomizationBeforeMinFitness = false;

            for (int i = 0; i < previousGenotypes.Length; i++)
            {
                if (i == 0 && keepBestGenotype)
                {
                    newGenotypes[0] = bestPreviousGenotypes[0];
                    continue;
                }
                Genotype offspring = CrossOver(bestPreviousGenotypes);
                newGenotypes[i] = offspring;
            }
            return newGenotypes;
        }

        private Genotype CrossOver(Genotype[] bestPreviousGenotypes)
        {
            Genotype offspring = new Genotype();
            double[][] newWeights = new double[bestPreviousGenotypes[0].Weights.Length][];
            int amountOfWeightLayers = bestPreviousGenotypes[0].Weights.Length;

            for (int i = 0; i < amountOfWeightLayers; i++)
            {
                double[] weightLayer = new double[bestPreviousGenotypes[0].Weights[i].Length];

                for (int j = 0; j < bestPreviousGenotypes[0].Weights[i].Length; j++)
                {
                    weightLayer[j] = CalculateNextWeight(bestPreviousGenotypes, i, j);
                }

                newWeights[i] = (weightLayer);
            }

            offspring.Weights = newWeights;

            return offspring;
        }

        private double CalculateNextWeight(Genotype[] bestPreviousGenotypes, int i, int j)
        {
            int totalFitness = 0;
            int sumOfFitnessSoFar = 0;
            foreach (var genotype in bestPreviousGenotypes)
            {
                totalFitness += genotype.fitness;
            }
            int randomFactor = Random.Range(0, totalFitness);
            foreach (var genotype in bestPreviousGenotypes)
            {
                sumOfFitnessSoFar += genotype.fitness;
                if (randomFactor < sumOfFitnessSoFar) return genotype.Weights[i][j];
            }
            Debug.Log("Error in genetic algorithm: sum of fitnesses never reched total fitness.");
            return 0;
        }

        private Genotype[] MutatePopulation(Genotype[] newGenotypes)
        {
            float mutationMin = (float)(-MutationAmount / 2.0f);
            float mutationMax = (float)(MutationAmount / 2.0f);

            foreach (Genotype genotype in newGenotypes)
            {
                if (keepBestGenotype && genotype == newGenotypes[0]) continue;

                foreach (var layer in genotype.Weights)
                {
                    for (int j = 0; j < layer.Length; j++)
                    {
                        if (Random.value < MutationProbability)
                        {
                            layer[j] += Random.Range(mutationMin, mutationMax);
                        }
                    }
                }
            }

            if (ExtraRandomizationBeforeMinFitness)
            {
                Debug.Log("Fitness score too low after five generations. Starting to randomize more.");

                int numberOfRandomGenotypes = (int)(newGenotypes.Length * (PercentOfRandomGenotypes / 100.0));
                for (int i = 0; i < numberOfRandomGenotypes; i++)
                {
                    newGenotypes[newGenotypes.Length - 1 - i].Weights = RandomizeWeights(newGenotypes[newGenotypes.Length - 1].Weights);
                }
            }
            return newGenotypes;
        }



        public Genotype[] SelectBestGenotypes(int numberOfBestGenotypes, Genotype[] previousGenotypes)
        {
            Genotype[] bestGenotypes = SortGenotypesByFitness(numberOfBestGenotypes, previousGenotypes);
            return bestGenotypes;
        }

        public Genotype[] SortGenotypesByFitness(int numberOfBestGenotypes, Genotype[] genotypes)
        {
            Genotype[] sortedGenotypes = genotypes.OrderByDescending(genotype => genotype.fitness).Take(numberOfBestGenotypes).ToArray();
            return sortedGenotypes;
        }

        private double[][] RandomizeWeights(double[][] weights)
        {
            double[][] newWeights = new double[weights.Length][];

            for (var i = 0; i < weights.Length; i++)
            {
                double[] weightLayer = new double[weights[i].Length];

                for (int j = 0; j < weightLayer.Length; j++)
                {
                    weightLayer[j] = HelperFunc.RandomWeight();
                }

                newWeights[i] = weightLayer;
            }

            return newWeights;
        }
    }
}