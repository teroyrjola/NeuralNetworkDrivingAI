using System.Linq;
using Boo.Lang;
using UnityEngine;


namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public int CurrentGeneration { get; set; }


        public int AmountOfBestGenotypesForParents = 2;
        public double MutationProbability = SimulationManagerScript.Instance.MutationProbability;
        public double MutationAmount = SimulationManagerScript.Instance.MutationAmount;
        public static int _CurrentGeneration = 0;

        private static readonly Random Random = new Random();

        public GeneticAlgorithm(int populationSize)
        {
            CurrentGeneration = _CurrentGeneration;
            _CurrentGeneration++;
        }

        public Genotype[] Start(Genotype[] genotypes)
        {
            Genotype[] newGenotypes = BreedNewPopulation(genotypes);
            return MutatePopulation(newGenotypes);
        }

        public Genotype[] BreedNewPopulation(Genotype[] previousGenotypes)
        {
            Genotype[] newGenotypes = new Genotype[previousGenotypes.Length];
            Genotype[] bestPreviousGenotypes = SelectBestGenotypes(AmountOfBestGenotypesForParents, previousGenotypes);

            int index = 0;

            while (newGenotypes.Length < previousGenotypes.Length)
            {
                Genotype offspring = CrossOver(bestPreviousGenotypes);
                newGenotypes[index] = offspring;
                index++;
            }

            return newGenotypes;
        }

        private Genotype CrossOver(Genotype[] bestPreviousGenotypes)
        {
            Genotype offspring = new Genotype();
            List<List<double>> newWeights = new List<List<double>>();
            int amountOfWeightLayers = bestPreviousGenotypes[0].Weights.Count;

            for (int i = 0; i < amountOfWeightLayers; i++)
            {
                List<double> weightLayer = new List<double>();

                for (int j = 0; j < bestPreviousGenotypes[0].Weights[i].Count; j++)
                {
                    weightLayer.Add(bestPreviousGenotypes[Random.Range(0, bestPreviousGenotypes.Length-1)].Weights[i][j]);
                }

                newWeights.Add(weightLayer);
            }

            offspring.Weights = newWeights;

            return offspring;
        }

        public Genotype[] MutatePopulation(Genotype[] newGenotypes)
        {
            var mutationMIN = (float) (-MutationAmount / 2.0f);
            var mutationMAX = (float) (MutationAmount / 2.0f);

            foreach (var genotype in newGenotypes)
            {
                foreach (var layer in genotype.Weights)
                {
                    for (int i = 0; i < layer.Count; i++)
                    {
                        if (Random.value < MutationProbability)
                        {
                            layer[i] += Random.Range(mutationMIN, mutationMAX);
                        }
                    }
                }
            }
            return newGenotypes;
        }

        public Genotype[] SelectBestGenotypes(int numberOfBestGenotypes, Genotype[] previousGenotypes)
        {
            Genotype[] bestGenotypes = new Genotype[numberOfBestGenotypes];
            SortGenotypesByFitness(previousGenotypes);
            for (int i = 0; i < numberOfBestGenotypes; i++)
            {
                bestGenotypes[i] = previousGenotypes[i];
            }
            return bestGenotypes;
        }

        public Genotype[] SortGenotypesByFitness(Genotype[] genotypes)
        {
            Genotype[] sortedGenotypes = genotypes.OrderByDescending(genotype => genotype.fitness).ToArray();
            return sortedGenotypes;
        }
    }
}