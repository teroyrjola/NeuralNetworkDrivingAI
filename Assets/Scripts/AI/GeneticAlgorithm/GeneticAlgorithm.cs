using System.Linq;
using Boo.Lang;
using UnityEngine;


namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public int CurrentGeneration { get; set; }
        public List<Genotype> Genotypes;

        public int AmountOfBestGenotypesForParents = 2;
        public double MutationProbability = 0.01;
        public double MutationAmount = 0.1;

        private static readonly Random Random = new Random();

        public GeneticAlgorithm(int populationSize)
        {
            CurrentGeneration = 0;
            Genotypes = new List<Genotype>(populationSize);
        }

        public void AddGenotype(List<List<Synapse>> synapses)
        {
            Genotypes.Add(new Genotype(synapses));
        }


        public void Start(List<Genotype> genotypes)
        {
            Genotypes = genotypes;
            BreedNewPopulation();

        }

        public void BreedNewPopulation()
        {
            List<Genotype> newGenotypes = new List<Genotype>();
            List<Genotype> bestPreviousGenotypes = new List<Genotype>(SelectBestGenotypes(AmountOfBestGenotypesForParents));

            while (newGenotypes.Count < Genotypes.Count)
            {
                Genotype offspring = CrossOver(bestPreviousGenotypes);
                newGenotypes.Add(offspring);
            }

            MutatePopulation();

            Genotypes = newGenotypes;
        }

        private Genotype CrossOver(List<Genotype> bestPreviousGenotypes)
        {
            Genotype offspring = new Genotype();
            List<List<double>> newWeights = new List<List<double>>();
            int amountOfWeightLayers = bestPreviousGenotypes[0].Weights.Count;

            for (int i = 0; i < amountOfWeightLayers; i++)
            {
                List<double> weightLayer = new List<double>();

                for (int j = 0; j < bestPreviousGenotypes[0].Weights[i].Count; j++)
                {
                    weightLayer.Add(bestPreviousGenotypes[Random.Range(0, bestPreviousGenotypes.Count-1)].Weights[i][j]);
                }

                newWeights.Add(weightLayer);
            }

            offspring.Weights = newWeights;

            return offspring;
        }

        public void MutatePopulation()
        {
            var mutationMIN = (float) (-MutationAmount / 2.0f);
            var mutationMAX = (float) (MutationAmount / 2.0f);

            foreach (var genotype in Genotypes)
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
        }

        public Genotype[] SelectBestGenotypes(int numberOfBestGenotypes)
        {
            Genotype[] bestGenotypes = new Genotype[numberOfBestGenotypes];
            SortGenotypesByFitness();
            for (int i = 0; i < numberOfBestGenotypes; i++)
            {
                bestGenotypes[i] = Genotypes[i];
            }
            return bestGenotypes;
        }

        public void SortGenotypesByFitness()
        {
            Genotypes = new List<Genotype>(Genotypes.OrderByDescending(genotype => genotype.fitness).ToList());
        }
    }
}