using System.Linq;
using Boo.Lang;

namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class Genotype
    {
        public List<List<double>> Weights;
        public double fitness;

        public Genotype(List<List<Synapse>> synapses)
        {
            fitness = 0;
            Weights = GetWeights(synapses);
        }

        public Genotype()
        {
            fitness = 0;
        }

        private List<List<double>> GetWeights(List<List<Synapse>> synapses)
        {
            List<List<double>> weights = new List<List<double>>();

            for (int i = 0; i < synapses.Count; i++)
            {
                weights[i] = (List<double>)synapses[i].Select(synapse => synapse.Weight);
            }

            return weights;
        }
    }
}