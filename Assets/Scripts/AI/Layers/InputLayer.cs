using Boo.Lang;

namespace Assets.Scripts
{
    public class InputLayer : ILayer
    {
        public List<Neuron> neurons { get; set; }

        public InputLayer(int numberOfNeurons)
        {
            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons.Add(new Neuron());
            }
        }
    }
}