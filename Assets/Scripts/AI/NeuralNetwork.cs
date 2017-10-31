using System;
using Boo.Lang;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        public  readonly int numberOfHiddenLayerNeurons = 7;

        public int hiddenLayers;
        public double[][] weights;
        public List<ILayer> layers;

        public NeuralNetwork(int hiddenLayers)
        {
            InitializeLayers();
            InitializeWeights();
        }

        private void InitializeLayers()
        {
            layers.Add(new InputLayer(5));
            for (int i = 0; i < hiddenLayers; i++)
            {
                layers.Add(new HiddenLayer(numberOfHiddenLayerNeurons));
            }
            layers.Add(new OutputLayer(2));
        }
        //intialize a random weight between neurons in layers next to eachother
        private void InitializeWeights()
        {
            List<List<Synapse>> synapses = new List<List<Synapse>>();

            for (int i = 0; i < layers.Count - 1; i++)
            {
                List<Synapse> layerOfSynapses = new List<Synapse>();

                for (int j = 0; i < layers[i].neurons.Count; j++)
                {
                    for (int k = 0; i < layers[i + 1].neurons.Count; k++)
                    {
                        layerOfSynapses.Add(new Synapse(layers[i].neurons[j], layers[i + 1].neurons[k]));
                    }
                }
                synapses.Add(layerOfSynapses);
            }
        }
    }
}