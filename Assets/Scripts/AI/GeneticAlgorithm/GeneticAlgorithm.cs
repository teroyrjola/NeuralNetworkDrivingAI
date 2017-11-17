using System.Linq;
using Boo.Lang;
using UnityEngine;


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

        public GeneticAlgorithm()
        {
            CurrentGeneration = _CurrentGeneration;
            _CurrentGeneration++;
        }

        public Genotype[] Start(Genotype[] genotypes)
        {
            Genotype[] newGenotypes = BreedNewPopulation(genotypes);
            newGenotypes =  MutatePopulation(newGenotypes);

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

                newWeights[i]= (weightLayer);
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
            return 0;
        }

        public Genotype[] MutatePopulation(Genotype[] newGenotypes)
        {
            var mutationMIN = (float) (-MutationAmount / 2.0f);
            var mutationMAX = (float) (MutationAmount / 2.0f);

            foreach (Genotype genotype in newGenotypes)
            {
                if (keepBestGenotype && genotype == newGenotypes[0]) continue;

                foreach (var layer in genotype.Weights)
                {
                    for (int j = 0; j < layer.Length; j++)
                    {
                        if (Random.value < MutationProbability)
                        {
                            layer[j] += Random.Range(mutationMIN, mutationMAX);
                        }
                    }
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
            Debug.Log(sortedGenotypes[0].fitness);
            return sortedGenotypes;
        }
    }
}