﻿using Assets.Scripts.AI.GeneticAlgorithm;
using UnityEngine;

namespace Assets.Scripts
{
    public class SimulationManagerScript : MonoBehaviour
    {
        public static SimulationManagerScript Instance { get; private set; }
        public GeneticAlgorithm GeneticAlgorithm;
        public int CarAmount;
        public int NumberOfHiddenLayers;
        public int NumberOfNeuronsPerHiddenLayer;
        public int AmountOfBestGenotypesForParents;
        public double MutationProbability;
        public double MutationAmount;
        private int _carsCrashed;
        public GameObject FirstCar;
        public GameObject Checkpoints;

        public Car[] cars;

        void Awake()
        {
            Instance = this;
            cars = new Car[CarAmount];

            for (int i = 0; i < CarAmount; i++)
            {
                GameObject newCar = Instantiate(FirstCar.gameObject);

                newCar.transform.position = newCar.transform.localPosition;
                newCar.transform.rotation = newCar.transform.localRotation;
                CarController newController = newCar.GetComponent<CarController>();
                cars[i] = (new Car(newController));
                newCar.SetActive(true);
            }
            FirstCar.SetActive(false);

            ResetCheckpoints();
        }

        public void CarCrash()
        {
            _carsCrashed++;

            if (_carsCrashed == CarAmount)
            {
                StartEvaluation(cars);
                _carsCrashed = 0;
            }
        }

        private void StartEvaluation(Car[] crashedCars)
        {
            GeneticAlgorithm = new GeneticAlgorithm();
            Genotype[] genotypes = new Genotype[CarAmount];

            for (var i = 0; i < crashedCars.Length; i++)
            {
                genotypes[i] = cars[i].Controller.Agent.Genotype;
            }

            Genotype[] newGenotypes = GeneticAlgorithm.Start(genotypes);

            for (int i = 0; i < cars.Length; i++)
            {
                cars[i].Controller.Agent.Genotype = newGenotypes[i];
                cars[i].Controller.Agent.ANN.synapses = CopyNewWeightsFromGAToANN(cars[i], newGenotypes[i]);
            }
            ResetCars();
            ResetCheckpoints();
        }

        private Synapse[][] CopyNewWeightsFromGAToANN(Car car, Genotype genotype)
        {
            Synapse[][] currentSynapses = car.Controller.Agent.ANN.synapses;

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
                car.Controller.GetComponentInParent<Transform>().position = FirstCar.transform.position;
                car.Controller.GetComponentInParent<Transform>().rotation = FirstCar.transform.rotation;
                car.Controller.Agent.IsAlive = true;
                car.Controller.Agent.CurrentGenFitness = 0;
                car.Controller.Agent.Genotype.fitness = 0;
                car.Controller.timeSinceLastCheckpoint = 0;
                car.Controller.ResetSensors();
                car.Controller.ShowSensors();
            }
        }

        private void ResetCheckpoints()
        {
            foreach (var checkpoint in Checkpoints.GetComponentsInChildren<CheckpointScript>())
            {
                checkpoint.SetRewardLeftToInitialValue();
            }
        }
    }
}
