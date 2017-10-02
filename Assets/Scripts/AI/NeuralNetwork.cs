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

        private void InitializeWeights()
        {
            int numberOfWeightLayers = layers.Count - 1;
            int[] numberOfWeightsPerLayer;
            List<List<double>> weights;

                foreach (var layer in layers)
                {
                    List<double> weightsInOneWeightLayer;
                    foreach (var neuron in layer.neurons)
                    {
                        weights.Add(new Random().NextDouble())
                    }
                    layer.neurons.Count;
                }
        }
    }
}