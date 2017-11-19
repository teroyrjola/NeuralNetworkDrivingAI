using Assets.Scripts.AI.GeneticAlgorithm;

namespace Assets.Scripts.AI
{
    public class Agent
    {
        public NeuralNetwork ANN { get; set; }

        public Genotype Genotype { get; set; }

        public int CurrentGenFitness;
        public bool IsAlive;

        public Agent()
        {
            ANN = new NeuralNetwork(SimulationManagerScript.Instance.NumberOfHiddenLayers);
            Genotype = new Genotype(ANN.synapses);
            IsAlive = true;
        }

        public void SetGenotypeFitness()
        {
            Genotype.fitness = CurrentGenFitness;
        }

    }
}