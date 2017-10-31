using System;
using System.Linq;
using Boo.Lang;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        public readonly int numberOfHiddenLayerNeurons = 3;

        public int hiddenLayers;
        public List<List<Synapse>> synapses;
        public List<ILayer> layers;

        public NeuralNetwork(int hiddenLayers)
        {
            InitializeLayers(hiddenLayers);
            InitializeWeights();
        }

        private void InitializeLayers(int hiddenLayers)
        {
            layers = new List<ILayer>();
            layers.Add(new InputLayer(5));
            for (int i = 0; i < hiddenLayers; i++)
            {
                layers.Add(new HiddenLayer(numberOfHiddenLayerNeurons));
            }
            layers.Add(new OutputLayer(4));
        }

        //intialize a random weight between neurons in layers next to eachother
        private void InitializeWeights()
        {
            synapses = new List<List<Synapse>>();

            for (int i = 0; i < layers.Count - 1; i++)
            {
                List<Synapse> layerOfSynapses = new List<Synapse>();

                for (int j = 0; j < layers[i].neurons.Count; j++)
                {
                    for (int k = 0; k < layers[i + 1].neurons.Count-1; k++)
                    {
                        layerOfSynapses.Add(new Synapse(layers[i].neurons[j], layers[i + 1].neurons[k]));
                    }
                }
                synapses.Add(layerOfSynapses);
            }
        }

        public void SetInputLayer(double[] sensorInput)
        {
            for (int i = 0; i < sensorInput.Length; i++)
            {
                layers[0].neurons[i].Value = sensorInput[i];
            }
        }

        public double[] ProcessInputs()
        {

            for (int i = 0; i < synapses.Count; i++)
            {
                foreach(var synapse in synapses[i])
                {
                    synapse.Calculate();
                }
                foreach (var neuron in layers[i].neurons)
                {
                    neuron.Value = HelperFunc.SigmoidFunction(neuron.Value);
                }
            }
            return layers[layers.Count - 1].neurons.Select(neuron => neuron.Value).ToArray();
        }
    }
}