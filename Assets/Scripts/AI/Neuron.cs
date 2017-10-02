using Boo.Lang;

namespace Assets.Scripts
{
    public class Neuron
    {
        public List<Dendrite> Dendrites { get; set; }
        public double Bias { get; set; }
    }
}