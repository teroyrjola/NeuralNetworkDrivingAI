using Boo.Lang;

namespace Assets.Scripts
{
    public class HiddenLayer : ILayer
    {
        public Neuron[] neurons { get; set; }

        public HiddenLayer(int numberOfNeurons)
        {
            neurons = new Neuron[numberOfNeurons];

            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons[i] = (new Neuron());
            }

        }
    }
}