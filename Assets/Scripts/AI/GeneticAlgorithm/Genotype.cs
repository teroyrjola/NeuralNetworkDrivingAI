using System.Linq;
using Boo.Lang;

namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class Genotype
    {
        public List<List<double>> Weights;
        public int fitness;

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
                List<double> weightLayer = new List<double>();

                for (int j = 0; j < synapses[i].Count; j++)
                    {
                        weightLayer.Add(synapses[i][j].Weight);
                    }
                weights.Add(weightLayer);
            }

            return weights;
        }
    }
}