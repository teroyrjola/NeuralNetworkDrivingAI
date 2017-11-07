using System.Collections;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.AI;
using Assets.Scripts.AI.GeneticAlgorithm;
using Boo.Lang;
using UnityEngine;

public class SimulationManagerScript : MonoBehaviour
{
    public static SimulationManagerScript Instance
    {
        get;
        private set;
    }
    public GeneticAlgorithm geneticAlgorithm;
    public int carAmount;
    public int numberOfHiddenLayers;
    public int numberOfNeuronsPerHiddenLayer;
    public double MutationProbability;
    public double MutationAmount;
    private int CarsCrashed;
    public GameObject firstCar;

    public Car[] cars;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        cars = new Car[carAmount];

        for (int i = 0; i < carAmount; i++)  //-1 because car number one is already on track
        {
            GameObject newCar = Instantiate(firstCar.gameObject);
            newCar.transform.position = newCar.transform.localPosition;
            newCar.transform.rotation = newCar.transform.localRotation;
            CarController newController = newCar.GetComponent<CarController>();
            cars[i] = (new Car(newController));
            newCar.SetActive(true);
        }
        firstCar.SetActive(false);
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
        geneticAlgorithm = new GeneticAlgorithm(carAmount);
        Genotype[] genotypes = new Genotype[carAmount];

        for (var i = 0; i < crashedCars.Length; i++)
        {
            genotypes[i] = cars[i].controller.Agent.genotype;
        }

        Genotype[] newGenotypes = geneticAlgorithm.Start(genotypes);

        cars = CopyNewWeightsFromGAToANN(crashedCars, newGenotypes);

        ResetCars();
    }

    private Car[] CopyNewWeightsFromGAToANN(Car[] crashedCars, Genotype[] genotypes)
    {
        for (int i = 0; i < crashedCars.Length; i++)
        {
            for (int j = 0; j < crashedCars[i].controller.Agent.ANN.synapses.Count; j++)
            {
                for (int k = 0; k < crashedCars[i].controller.Agent.ANN.synapses[j].Count; k++)
                {
                    crashedCars[i].controller.Agent.ANN.synapses[j][k].Weight = genotypes[i].Weights[j][k];
                }

            }
        }

        return crashedCars;
    }

    private void ResetCars()
    {
        foreach (var car in cars)
        {
            car.controller.GetComponentInParent<Transform>().position = firstCar.transform.position;
            car.controller.GetComponentInParent<Transform>().rotation = firstCar.transform.rotation;
            car.controller.Agent.IsAlive = true;
            car.controller.timeSinceLastCheckpoint = 0;
        }
    }
}
