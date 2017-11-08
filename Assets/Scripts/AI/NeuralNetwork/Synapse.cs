using System;

namespace Assets.Scripts
{
    public class Synapse
    {
        public Neuron InputNeuron { get; set; }
        public Neuron OutputNeuron { get; set; }
        public double Weight { get; set; }
        public double WeightDelta { get; set; }

        public Synapse(Neuron inputNeuron, Neuron outputNeuron)
        {
            InputNeuron = inputNeuron;
            OutputNeuron = outputNeuron;
            Weight = HelperFunc.RandomWeight();
        }

        public void Calculate()
        {
            OutputNeuron.Value += InputNeuron.Value * Weight;
        }

        public void SetWeight(double weight)
        {
            Weight = weight;
        }
    }
}