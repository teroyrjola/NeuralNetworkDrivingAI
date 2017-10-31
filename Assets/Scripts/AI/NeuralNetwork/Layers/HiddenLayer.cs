using Boo.Lang;

namespace Assets.Scripts
{
    public class HiddenLayer : ILayer
    {
        public List<Neuron> neurons { get; set; }

        public HiddenLayer(int numberOfNeurons)
        {
            neurons = new List<Neuron>();
            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons.Add(new Neuron());
            }
        }
    }
}