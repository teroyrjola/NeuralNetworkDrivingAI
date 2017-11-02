using System;
using System.Linq;
using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        private readonly Neuron BiasNeuron;
        public readonly int numberOfHiddenLayerNeurons = 3;

        public static double bias;
        //public int hiddenLayers;
        public List<List<Synapse>> synapses;
        public List<ILayer> layers;


        public NeuralNetwork(int hiddenLayers)
        {
            InitializeLayers(hiddenLayers);
            BiasNeuron = new Neuron(bias);
            InitializeWeights();
        }

        private void InitializeLayers(int hiddenLayers)
        {
            layers = new List<ILayer> {new InputLayer(5)};

            for (int i = 0; i < hiddenLayers; i++)
            {
                layers.Add(new HiddenLayer(numberOfHiddenLayerNeurons));
            }
            layers.Add(new OutputLayer(2));
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
                for (int j = 0; j < layers[i + 1].neurons.Count - 1; j++)
                {
                    layerOfSynapses.Add(new Synapse(BiasNeuron, layers[i + 1].neurons[j])); //bias
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
            //apply the activation function also to the output layer's neurons
            foreach (var neuron in layers[layers.Count - 1].neurons)
            {
                neuron.Value = HelperFunc.SigmoidFunction(neuron.Value);
            }
            return layers[layers.Count - 1].neurons.Select(neuron => neuron.Value).ToArray();
        }
    }
}