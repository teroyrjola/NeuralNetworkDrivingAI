using Boo.Lang;

namespace Assets.Scripts
{
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