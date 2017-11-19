using Assets.Scripts.AI.GeneticAlgorithm;

namespace Assets.Scripts.AI
{
    public class Agent
    {
        public NeuralNetwork ANN { get; set; }

        public Genotype genotype { get; set; }

        public int CurrentGenFitness;
        public bool IsAlive;

        public Agent()
        {
            ANN = new NeuralNetwork(SimulationManagerScript.Instance.numberOfHiddenLayers);
            genotype = new Genotype(ANN.synapses);
            IsAlive = true;
        }

        public void SetGenotypeFitness()
        {
            genotype.fitness = CurrentGenFitness;
        }

    }
}