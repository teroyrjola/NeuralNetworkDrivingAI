using Boo.Lang;

namespace Assets.Scripts
{
    public class NeuralNetwork
    {
        public int hiddenLayers;
        public List<ILayer> layers;

        public NeuralNetwork(int hiddenLayers)
        {
            layers.Add(new InputLayer(5));
            layers.Add(new HiddenLayer(7));
            layers.Add(new OutputLayer(2));
        }
    }
}