using System.Linq;
using Boo.Lang;
using UnityEngine;


namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public int CurrentGeneration { get; set; }


        public int AmountOfBestGenotypesForParents = 2;
        private bool keepBestGenotype = true;
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
            Debug.Log(bestPreviousGenotypes[0].fitness +" "+bestPreviousGenotypes[1].fitness);
            Genotype offspring = new Genotype();
            double[][] newWeights = new double[bestPreviousGenotypes.Length][];
            int amountOfWeightLayers = bestPreviousGenotypes[0].Weights.Length;

            for (int i = 0; i < amountOfWeightLayers; i++)
            {
                double[] weightLayer = new double[bestPreviousGenotypes[0].Weights[i].Length];

                for (int j = 0; j < bestPreviousGenotypes[0].Weights[i].Length; j++)
                {
                    weightLayer[j] = (bestPreviousGenotypes[Random.Range(0, bestPreviousGenotypes.Length)].Weights[i][j]);
                }

                newWeights[i]= (weightLayer);
            }

            offspring.Weights = newWeights;

            return offspring;
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
            return sortedGenotypes;
        }
    }
}