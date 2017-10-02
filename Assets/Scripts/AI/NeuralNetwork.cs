using Boo.Lang;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        public int hiddenLayers;
        public List<Layer> layers;

        public NeuralNetwork(int hiddenLayers)
        {
            layers.Add()
        }
    }

    interface ILayer
    {
        List<Neuron> neurons { get; set; }
    }

    public class HiddenLayer : ILayer
    {
        public List<Neuron> neurons { get; set; }

        public HiddenLayer(int numberOfNeurons)
        {
            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons.Add(new Neuron());
            }
        }


    }
}