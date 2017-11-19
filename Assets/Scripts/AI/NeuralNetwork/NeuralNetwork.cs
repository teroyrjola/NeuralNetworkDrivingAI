using System.Linq;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        private readonly Neuron BiasNeuron = new Neuron(1);
        private readonly int NumberOfInputNeurons = 5; //5 sensors on the car
        private readonly int NumberOfOutputNeurons = 2; //horizontal and vertical
        public readonly int numberOfHiddenLayerNeurons = SimulationManagerScript.Instance.NumberOfNeuronsPerHiddenLayer;

        public static double bias;
        //public int hiddenLayers;
        public Synapse[][] synapses;
        public ILayer[] layers;


        public NeuralNetwork(int hiddenLayers)
        {
            InitializeLayers(hiddenLayers);
            BiasNeuron = new Neuron(bias);
            InitializeWeights();
        }

        private void InitializeLayers(int hiddenLayers)
        {
            layers = new ILayer[2 + hiddenLayers];
            layers[0] = new InputLayer(NumberOfInputNeurons);
            for (int i = 1; i < hiddenLayers +1 ; i++)
            {
                layers[i] = new HiddenLayer(numberOfHiddenLayerNeurons);
            }
            layers[layers.Length -1] = (new OutputLayer(NumberOfOutputNeurons));
        }

        //intialize a random weight between neurons in layers next to eachother
        private void InitializeWeights()
        {
            synapses = new Synapse[layers.Length-1][];

            for (int i = 0; i < layers.Length - 1; i++)
            {
                Synapse[] layerOfSynapses = new Synapse[layers[i].neurons.Length];

                for (int j = 0; j < layers[i].neurons.Length; j++)
                {
                    for (int k = 0; k < layers[i + 1].neurons.Length-1; k++)
                    {
                        layerOfSynapses[j] = new Synapse(layers[i].neurons[j], layers[i + 1].neurons[k]);
                    }
                }
                for (int j = 0; j < layers[i + 1].neurons.Length - 1; j++)
                {
                    layerOfSynapses[layerOfSynapses.Length -1] = new Synapse(BiasNeuron, layers[i + 1].neurons[j]); //bias
                }
                synapses[i] = (layerOfSynapses);
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
            for (int i = 0; i < synapses.Length; i++)
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
            foreach (var neuron in layers[layers.Length - 1].neurons)
            {
                neuron.Value = HelperFunc.SigmoidFunction(neuron.Value);
            }
            return layers[layers.Length - 1].neurons.Select(neuron => neuron.Value).ToArray();
        }
    }
}