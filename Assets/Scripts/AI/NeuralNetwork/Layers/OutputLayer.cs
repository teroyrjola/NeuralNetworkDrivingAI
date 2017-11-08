using Boo.Lang;

namespace Assets.Scripts
{

    public class OutputLayer : ILayer
    {
        public Neuron[] neurons { get; set; }

        public OutputLayer(int numberOfNeurons)
        {
            neurons = new Neuron[numberOfNeurons];
            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons[i] = (new Neuron());
            }
        }

    }
}