namespace Assets.Scripts
{
    public class InputLayer : ILayer
    {
        public Neuron[] neurons { get; set; }

        public InputLayer(int numberOfNeurons)
        {
            neurons = new Neuron[numberOfNeurons];
            for (int i = 0; i < numberOfNeurons; i++)
            {
                neurons[i] = (new Neuron());
            }
        }
    }
}