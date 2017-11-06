using System.Collections;
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
    private int CarsCrashed;
    public GameObject firstCar;

    public Car[] cars;

	// Use this for initialization
	void Awake ()
	{
	    Instance = this;
	    cars = new Car[carAmount];

	    for (int i = 0; i < carAmount; i++)  //-1 because car number one is already on track
	    {
	        GameObject newCar = Instantiate(firstCar.gameObject);
	        newCar.transform.position = newCar.transform.localPosition;
	        newCar.transform.rotation = newCar.transform.localRotation;
	        CarController newController = newCar.GetComponent<CarController>();
	        cars [i] = (new Car(newController));
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
        List<Genotype> genotypes = new List<Genotype>();
        Debug.Log(cars[0].controller.Agent.ANN.layers[1].neurons[1].Value);

        foreach (var car in crashedCars)
        {
            genotypes.Add(car.controller.Agent.genotype);
        }

        geneticAlgorithm.Start(genotypes);

        ResetCars();
    }

    private void ResetCars()
    {
        foreach (var car in cars)
        {
            car.controller.GetComponentInParent<Transform>().position = firstCar.transform.position;
            car.controller.GetComponentInParent<Transform>().rotation = firstCar.transform.rotation;
            Debug.Log("auto resetoitu" + car.controller.GetComponent<CarController>().Agent.IsAlive + " " + car.carID);
            car.controller.Agent.IsAlive = true;
            car.controller.timeSinceLastCheckpoint = 0;
        }
    }
}
