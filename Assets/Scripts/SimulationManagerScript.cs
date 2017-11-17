using System.Linq;
using Assets.Scripts.AI.GeneticAlgorithm;
using UnityEngine;

namespace Assets.Scripts
{
    public class SimulationManagerScript : MonoBehaviour
    {
        public static SimulationManagerScript Instance { get; private set; }
        public GeneticAlgorithm geneticAlgorithm;
        public int carAmount;
        public int numberOfHiddenLayers;
        public int numberOfNeuronsPerHiddenLayer;
        public int AmountOfBestGenotypesForParents;
        public double MutationProbability;
        public double MutationAmount;
        private int CarsCrashed;
        public GameObject firstCar;
        public GameObject checkpoints;

        public Car[] cars;

        void Awake()
        {
            Instance = this;
            cars = new Car[carAmount];

            for (int i = 0; i < carAmount; i++) //-1 because car number one is already on track
            {
                GameObject newCar = Instantiate(firstCar.gameObject);

                newCar.transform.position = newCar.transform.localPosition;
                newCar.transform.rotation = newCar.transform.localRotation;
                CarController newController = newCar.GetComponent<CarController>();
                cars[i] = (new Car(newController));
                newCar.SetActive(true);
            }
            firstCar.SetActive(false);

            ResetCheckpoints();
        }

        public void CarCrash()
        {
            CarsCrashed++;

            if (CarsCrashed == carAmount)
            {
                StartEvaluation(cars);
                CarsCrashed = 0;
            }
        }

        private void StartEvaluation(Car[] crashedCars)
        {
            geneticAlgorithm = new GeneticAlgorithm();
            Genotype[] genotypes = new Genotype[carAmount];

            for (var i = 0; i < crashedCars.Length; i++)
            {
                genotypes[i] = cars[i].controller.Agent.genotype;
            }

            Genotype[] newGenotypes = geneticAlgorithm.Start(genotypes);

            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].controller.Agent.genotype = newGenotypes[i];
                cars[i].controller.Agent.ANN.synapses = CopyNewWeightsFromGAToANN(cars[i], newGenotypes[i]);
            }
            ResetCars();
            ResetCheckpoints();
        }

        private Synapse[][] CopyNewWeightsFromGAToANN(Car car, Genotype genotype)
        {
            Synapse[][] currentSynapses = car.controller.Agent.ANN.synapses;

            for (int i = 0; i < currentSynapses.Length; i++)
            {
                for (int j = 0; j < currentSynapses[i].Length; j++)
                {
                    currentSynapses[i][j].SetWeight(genotype.Weights[i][j]);
                }
            }

            return currentSynapses;
        }

        private void ResetCars()
        {
            foreach (var car in cars)
            {
                car.controller.GetComponentInParent<Transform>().position = firstCar.transform.position;
                car.controller.GetComponentInParent<Transform>().rotation = firstCar.transform.rotation;
                car.controller.Agent.IsAlive = true;
                car.controller.Agent.CurrentGenFitness = 0;
                car.controller.Agent.genotype.fitness = 0;
                car.controller.timeSinceLastCheckpoint = 0;
                car.controller.ResetSensors();
                car.controller.ShowSensors();
            }
        }

        private void ResetCheckpoints()
        {
            foreach (var checkpoint in checkpoints.GetComponentsInChildren<CheckpointScript>())
            {
                checkpoint.SetRewardLeftToInitialValue();
            }
        }
    }
}
