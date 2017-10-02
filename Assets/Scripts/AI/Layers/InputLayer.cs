using Boo.Lang;

namespace Assets.Scripts
{
    public partial class HiddenLayer : ILayer
    {
        public partial class InputLayer : ILayer
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