namespace Assets.Scripts.AI
{
    public class Agent
    {
        public NeuralNetwork ANN { get; set; }

        public bool IsAlive;

        public Agent()
        {
            ANN = new NeuralNetwork(5);
        }
    }
}