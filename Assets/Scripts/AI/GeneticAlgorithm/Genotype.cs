namespace Assets.Scripts.AI.GeneticAlgorithm
{
    public class Genotype
    {
        public double[][] Weights;
        public int fitness;

        public Genotype(Synapse[][] synapses)
        {
            fitness = 0;
            Weights = GetWeights(synapses);
        }

        public Genotype()
        {
            fitness = 0;
        }

        private double[][] GetWeights(Synapse[][] synapses)
        {
            double[][] weights = new double[synapses.Length][];

            for (int i = 0; i < synapses.Length; i++)
            {
                double[] weightLayer = new double[synapses[i].Length];

                for (int j = 0; j < synapses[i].Length; j++)
                    {
                        weightLayer[j] = (synapses[i][j].Weight);
                    }
                weights[i] = (weightLayer);
            }

            return weights;
        }
    }
}